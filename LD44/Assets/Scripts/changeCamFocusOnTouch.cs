using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCamFocusOnTouch : MonoBehaviour
{
    [SerializeField] GameObject cameraToDisable;
    [SerializeField] BossBehaviour boss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            cameraToDisable.SetActive(false);
            boss.setTarget(other.transform);
        }
    }
}
