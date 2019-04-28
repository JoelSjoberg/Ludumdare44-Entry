using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHp;
    float hp;
    [HideInInspector]public bool dead;

    // The script-holder is attacking, touch means damage
    [HideInInspector]public bool dangerous = false;

    [HideInInspector]public bool invulnerable = false;

    // if no invulnerability is allowed: set invulTime to 0
    [SerializeField] float invulTime = 1f;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        timer = invulTime;
        hp = maxHp;
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
                transform.GetComponent<PlayerMovement>().stun();
            } 
        }

        if (hp <= 0)
        {
            dead = true;
            if (transform.tag != "Player")
            {
                transform.GetComponent<Rigidbody>().isKinematic = false;
                Destroy(this.gameObject, 0.5f);
            } 
        } 
    }

    IEnumerator destroydelay(float delay = 1f)
    {
        float timer = 0;

        while (timer < delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
        yield return 0;
    }
    public void heal()
    {
        if (hp < maxHp)
        {
            hp += 1;
        }
    }
    public void revive()
    {
        hp = maxHp;
        dead = false;
        dangerous = false;
        invulnerable = false;
    }


    /*
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!invulnerable)
        {

            print("Health collision between: " + this.tag + " and "+ hit.transform.tag);
            // If both are attacking, deal no damage and bounce them off each other
            if (hit.transform.tag == "Enemy" && dangerous && hit.transform.GetComponent<Health>().dangerous)
            {
                hit.transform.GetComponent<Rigidbody>().AddForce((transform.position - hit.transform.position).normalized * 2);
                GetComponent<Rigidbody>().AddForce((hit.transform.position - transform.position).normalized * 2);
            }
            if (hit.transform.tag == "Enemy" && dangerous && !hit.transform.GetComponent<Health>().invulnerable)
            {
                hit.transform.GetComponent<Rigidbody>().AddForce((transform.position - hit.transform.position).normalized * 2);

                // Deal damage and gain 1 hp
                hit.transform.GetComponent<Health>().takeDamage();
                heal();
            }
        }
    }
    */
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
                if ((collision.transform.tag == "Enemy" || collision.transform.tag == "Player") && dangerous && !collision.transform.GetComponent<Health>().invulnerable)
                {
                collision.transform.GetComponent<Rigidbody>().AddForce((transform.position - collision.transform.position).normalized * 2);

                startShake(collision.transform);
                // Deal damage and gain 1 hp
                collision.transform.GetComponent<Health>().takeDamage();
                heal();
                print(transform.tag + " Attacked " + collision.transform.tag + ", collision hp: " + collision.transform.GetComponent<Health>().hp);
            }
            }
    }

    public void startShake(Transform t)
    {
        StartCoroutine(shake(0.2f, 0.02f, t));
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
