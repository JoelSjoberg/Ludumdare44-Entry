using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Health : MonoBehaviour
{
    [SerializeField] float maxHp;
    float hp;
    [HideInInspector]public bool dead;

    // The script-holder is attacking, touch means damage
    [HideInInspector]public bool dangerous = false;

    [HideInInspector]public bool invulnerable = false;

    [SerializeField] Image hpBar;

    // if no invulnerability is allowed: set invulTime to 0
    [SerializeField] float invulTime = 1f;
    float timer = 0;


    float originalBarWidth;
    // Start is called before the first frame update
    void Start()
    {
        timer = invulTime;
        hp = maxHp;

        if (hpBar != null)
        {
            originalBarWidth = hpBar.rectTransform.sizeDelta.x;
        }

        // the player can start with a penalty (only 3 hp) and increase that to max hp (about 10?)
        if (tag == "Player")
        {
            hp = 3;
            hpBar.rectTransform.sizeDelta = new Vector2(hp * originalBarWidth / maxHp, hpBar.rectTransform.sizeDelta.y);
        }
    }

    IEnumerator countInvulTime()
    {
        timer = 0;
        invulnerable = true;

        while (timer < invulTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        invulnerable = false;
        yield return 0;
    }
    // Simple hp system
    public void takeDamage()
    {

        
        if (!dead && !invulnerable)
        {
            hp -= 1;
            StartCoroutine(countInvulTime());

            // if the player is hit
            if (transform.tag == "Player")
            {
                audioManager.playSound("hurt");
                transform.GetComponent<PlayerMovement>().stun();
            } 
        }

        if (hpBar != null)
        {
            hpBar.rectTransform.sizeDelta = new Vector2(hp * originalBarWidth / maxHp, hpBar.rectTransform.sizeDelta.y);
        }
        if (hp <= 0)
        {
            dead = true;
            if (transform.tag != "Player")
            {
                transform.GetComponent<Rigidbody>().isKinematic = false;

                if (this.name != "boss")
                {
                    Destroy(this.gameObject, 0.5f);
                }
                else
                {
                    Invoke("loadEndScene", 3f);
                }
            } 
        } 
    }

    void loadEndScene()
    {
        SceneManager.LoadScene("thankyou");
    }

    public void heal()
    {
        if (hp < maxHp)
        {
            hp += 1;
        }
        if (hpBar != null)
        {
            hpBar.rectTransform.sizeDelta = new Vector2(hp * originalBarWidth / maxHp, hpBar.rectTransform.sizeDelta.y);
        }
    }
    public void revive()
    {
        hp = maxHp;
        dead = false;
        dangerous = false;
        invulnerable = false;
        if (hpBar != null)
        {
            hpBar.rectTransform.sizeDelta = new Vector2(hp * originalBarWidth / maxHp, hpBar.rectTransform.sizeDelta.y);
        }
    }
    // Same for non character controller entities
    private void OnCollisionEnter(Collision collision)
    {
            if (!invulnerable && (collision.transform.tag == "Enemy" || collision.transform.tag == "Player"))
            {

                //print("Health collision between: " + this.tag + " and " + collision.transform.tag);
                // If both are attacking, deal no damage and bounce them off each other
                if ((collision.transform.tag == "Enemy" || collision.transform.tag == "Player") && dangerous && collision.transform.GetComponent<Health>().dangerous)
                {
                    print("The characters attacked each other!");
                    startShake(collision.transform);
                    startShake(transform);
                    
                }
                else if ((collision.transform.tag == "Enemy" || collision.transform.tag == "Player") && dangerous && !collision.transform.GetComponent<Health>().invulnerable)
                {
                    collision.transform.GetComponent<Rigidbody>().AddForce((transform.position - collision.transform.position).normalized * 2);

                    startShake(collision.transform);
                    // Deal damage and gain 1 hp
                    print("Should heal now");
                    collision.transform.GetComponent<Health>().takeDamage();
                    heal();
                    print(transform.tag + " Attacked " + collision.transform.tag + ", collision hp: " + collision.transform.GetComponent<Health>().hp);
            }
            }
    }

    [SerializeField] float shakeDuration = 0.2f, shakeIntensity = 0.02f;
    public void startShake(Transform t)
    {
        StartCoroutine(shake(shakeDuration, shakeIntensity, t));
    }
    IEnumerator shake(float t, float intensity, Transform transform, float timer = 0)
    {

        Vector3 origin = transform.position;

        while (timer < t)
        {
            transform.position = origin;
            timer += Time.deltaTime;
            transform.position += (Vector3)Random.insideUnitCircle * intensity;
            yield return null;
        }
        transform.position = origin;
    }

}
