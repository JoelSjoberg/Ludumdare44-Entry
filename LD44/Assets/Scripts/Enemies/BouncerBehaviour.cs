using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerBehaviour : MonoBehaviour
{

    Rigidbody rb;

    [SerializeField] float upwardforce = 100;
    float timer = 0f;

    [SerializeField]float interval = 1.2f;
    [SerializeField] float downwardAccel = 0.3f;
    bool hitGround = false;
    float seismicDuration = 0.5f;
    float s_timer;

    Health health;

    [SerializeField]Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        interval += Random.Range(-1.2f, 1.2f);
        health = GetComponent<Health>();
    }
    // Update is called once per frame
    void Update()
    {
        // Will act as cooldown for the shock, OP otherwise
        if (s_timer < seismicDuration) s_timer += Time.deltaTime;
        else
        {
            hitGround = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity -= new Vector3(0, Time.deltaTime * downwardAccel, 0);
            health.dangerous = true;
        }
        else health.dangerous = false; 

        if (timer < interval) timer += Time.deltaTime;
        else
        {
            if (!health.dead)
            {

                Bounce();
                timer = 0;
            }
            else
            {
                anim.SetTrigger("dead");
            }
        }
    }

    void Bounce()
    {
        anim.SetTrigger("bounce");
        hitGround = false;
        
        rb.AddForce(new Vector3(0, upwardforce, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && hitGround)
        {
            other.transform.GetComponent<Health>().takeDamage();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Untagged" && !hitGround && s_timer > seismicDuration && !health.dead)
        {
            hitGround = true;
            s_timer = 0;
            StartCoroutine(shake(time, intensity, collision.transform));
        }
        if (collision.transform.tag == "Enemy" && !health.dead)
        {
            Destroy(collision.transform.gameObject);
        }
        
    }

    [SerializeField]float time = 0.4f, intensity = 0.08f;

    public void startShake(Transform t)
    {
            StartCoroutine(shake(time, intensity, t));
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
