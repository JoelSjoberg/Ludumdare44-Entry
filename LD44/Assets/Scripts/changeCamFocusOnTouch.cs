using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCamFocusOnTouch : MonoBehaviour
{
    [SerializeField] GameObject cameraToDisable;
    [SerializeField] GameObject cameraToActivate;
    [SerializeField] BossBehaviour boss;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            cameraToActivate.SetActive(true);
            cameraToDisable.SetActive(false);
            
            boss.setTarget(other.transform);
        }
    }
}
