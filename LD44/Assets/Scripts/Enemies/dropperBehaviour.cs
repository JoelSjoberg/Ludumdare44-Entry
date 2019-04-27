using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropperBehaviour : MonoBehaviour
{
    float dropTimer = 0;
    float[] dropTimes = { 0.2f, 0.7f};
    Transform playerTransform;
    [SerializeField]GameObject bomb;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        dropTimer += Time.deltaTime;

        if (playerTransform != null)
        {
            if (dropTimer >= Random.Range(dropTimes[0], dropTimes[1]))
            {
                dropTimer = 0;
                drop();

                rb.AddForce(new Vector3((playerTransform.position.x - transform.position.x) * 100, 0, 0));
            }

            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(playerTransform.position.x, transform.position.y, transform.position.z), 0f);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            playerTransform = null;
        }
    }

    void drop()
    {
        Instantiate(bomb, transform.position, Quaternion.identity);
    }
}
