using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{

    Transform playerTransform;
    [SerializeField]Transform arenaCenter;
    bool chooseNext = true;
    Rigidbody rb;

    bool dangerous = false;

    [SerializeField]GameObject bomb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void setTarget(Transform t)
    {
        playerTransform = t;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerTransform != null)
        {
            if (chooseNext)
            {
                if (hoverStackCounter < hoverStack)
                {
                    if (hoverStackCounter + 1 == hoverStack) StartCoroutine(hoverAndAttack(3f));
                    else StartCoroutine(hoverAndAttack());
                    hoverStackCounter += 1;
                }

                // Choose random attack if no othet stack attack is active
                else
                {
                    float rand = Random.Range(0.0f, 3f);
                    if (rand <= 1)
                    {
                        stackedHoverAttack();
                    }
                    else if (rand > 1 && rand <= 2) StartCoroutine(dropBombs());
                    else
                    {
                       StartCoroutine(bounceAttack());
                    }
                } 

            }
        }
    }

    float hoverStack = 3, hoverStackCounter = 3;

    void stackedHoverAttack()
    {
        hoverStackCounter = 0;
    }

    // Once after all any attack he will hover towards the center for a few seconds
    IEnumerator hoverToCenter(float duration)
    {
        chooseNext = false;
        dangerous = false;
        float timer = 0;
        rb.useGravity = false;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            rb.AddForce((arenaCenter.position - transform.position).normalized * 5f);
            yield return null;
        }
        rb.useGravity = true;
        chooseNext = true;
        yield return null;
    }

    IEnumerator hoverAndAttack(float restTime = 0.7f)
    {
        // Do not choose another attack while performing this
        chooseNext = false;
        // Follow for duration
        float timer = 0;
        float duration = 1.3f;

        dangerous = true;
        float speed = 50f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                rb.AddForce((playerTransform.position - transform.position).normalized * speed);
                yield return null;
            }
            StartCoroutine(hoverToCenter(restTime));
            yield return 0;
    }

    IEnumerator bounceAttack()
    {
        chooseNext = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        float timer = 0, countdown = 3f;
        while (timer < countdown)
        {
            timer += Time.deltaTime;

            // Warn the player by moving above
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerTransform.position.x, playerTransform.position.y + 10, 0), 0.1f);
            yield return null;
        }

        // Once done, charge downward

        rb.isKinematic = false;
        dangerous = true;
        while(dangerous)
        {
            rb.velocity += Vector3.down * 2;
            yield return null;
        }
        StartCoroutine(hoverToCenter(2f));
        yield return 0;

    }


    IEnumerator dropBombs()
    {

        chooseNext = false;
        // drop "times" bombs with "interval" between each bomb
        float timer = 0, interval = 0.3f, times = 5, counter = 0;

        // hover 10 units above the players y position when the attack started
        float y_pos = playerTransform.position.y + 10;
        rb.isKinematic = true;
        while (counter < times)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(playerTransform.position.x, y_pos, 0), 0.1f);
            timer += Time.deltaTime;

            if (timer >= interval)
            {
                timer = 0;
                counter += 1;
                dropBomb();
            }
            yield return null;
        }

        rb.isKinematic = false;
        if (Random.Range(0, 3) > 1.5)
        {
            // at random, charge down like the bounce attack
            print("Charge down");
            rb.isKinematic = false;
            dangerous = true;
            while (dangerous)
            {
                rb.velocity += Vector3.down * 2;
                yield return null;
            }
            StartCoroutine(hoverToCenter(2f));
            yield return 0;
        }
        else
        {
            dangerous = false;
            StartCoroutine(hoverToCenter(2f));
            yield return 0;

        }

    }

    void dropBomb()
    {
        Instantiate(bomb, transform.position, Quaternion.identity);
    }

    [SerializeField] float time = 0.4f, intensity = 0.08f;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && dangerous)
        {
            dangerous = false;
            print("Player should take damage from boss bounce");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (dangerous)
        {
            if (collision.transform.tag == "Untagged")
            {
                StartCoroutine(shake(time, intensity, collision.transform));
                dangerous = false;
            }
            if (collision.transform.tag == "Enemy")
            {
                Destroy(collision.transform.gameObject);
            }
        }
    }

}
