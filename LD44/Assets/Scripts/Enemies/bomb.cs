﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player") other.transform.GetComponent<Health>().takeDamage();
        audioManager.playSound("bomb");
        Destroy(this.gameObject);
    }
}
