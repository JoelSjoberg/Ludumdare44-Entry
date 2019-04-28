using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeCamFocusOnTouch : MonoBehaviour
{
    // Should really be called "setUpBossBattle" but too late now...

    bool activated = false;
    [SerializeField] GameObject cameraToDisable;
    [SerializeField] GameObject cameraToActivate;
    [SerializeField] BossBehaviour boss;

    [SerializeField] GameObject UIElements;

    [SerializeField] audioManager am;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !activated)
        {
            activated = true;
            UIElements.SetActive(true);
            cameraToActivate.SetActive(true);
            cameraToDisable.SetActive(false);
            
            boss.setTarget(other.transform);

            am.play("boss");
        }
    }
}
