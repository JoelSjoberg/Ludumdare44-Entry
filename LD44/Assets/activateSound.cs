using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateSound : MonoBehaviour
{

    [SerializeField]audioManager am;
    [SerializeField] string ost;
    [SerializeField] bool disabler;
    private void OnTriggerEnter(Collider other)
    {
        if (!disabler)
        {
            print("player entered");
            if (other.tag == "Player") am.SoundFadeIn(ost);

        }
        else
        {
            am.fadeAndStop("drums");
            am.fadeAndStop("viola");
            am.fadeAndStop("baroche");
            am.fadeAndStop("bass");


        }
    }
}
