using UnityEngine;

public class player_detector : MonoBehaviour
{
    bool player_detected = false;
    Transform player_t;



    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            player_detected = true;
            player_t = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player") player_detected = false;
    }

    public bool playerDetected()
    {
        return player_detected;
    }
    public Transform getPlayer()
    {
        return player_t;
    }
}
