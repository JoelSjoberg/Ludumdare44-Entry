using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCamFocusOnTouch : MonoBehaviour
{
    [SerializeField] GameObject cameraToDisable;
    [SerializeField] GameObject cameraToActivate;
    [SerializeField] BossBehaviour boss;

    [SerializeField] GameObject UIElements;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            UIElements.SetActive(true);
            cameraToActivate.SetActive(true);
            cameraToDisable.SetActive(false);
            
            boss.setTarget(other.transform);
        }
    }
}
