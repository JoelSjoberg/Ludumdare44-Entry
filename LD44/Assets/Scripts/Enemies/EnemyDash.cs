using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDash : MonoBehaviour
{
    Vector3 startPos;
    player_detector pd;
    CharacterController cc;

    float timer = 0;
    float recoveryTimer = 0;
    float leadup = 0.5f;
    float recover = 1f;

    float speed;

    bool attacking = false;
    Rigidbody rb;
    Health h;

    // Start is called before the first frame update
    void Start()
    {
        h = GetComponent<Health>();
        rb = GetComponent<Rigidbody>();
        pd = GetComponentInChildren<player_detector>();
        cc = GetComponent<CharacterController>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (recoveryTimer > 0)
        {
            recoveryTimer -= Time.deltaTime;
        }
        else if(pd.playerDetected())
        {
            
                if (timer < leadup) timer += Time.deltaTime;

                // Re-check statement just in case the player is out of reach 
                else if (pd.playerDetected())
                {
                    
                    rb.AddForce(pd.getPlayer().position - transform.position);


                    if (rb.velocity.magnitude > 0.1f) h.dangerous = true;
                    else h.dangerous = false;
            }
            
        } 
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            recoveryTimer = recover;
            h.dangerous = false;
            timer = 0;
        }
    }
}
