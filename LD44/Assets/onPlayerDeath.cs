using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class onPlayerDeath : MonoBehaviour
{
    [SerializeField]Transform spawn;
    Health h;
    [SerializeField]float delay = 2f;
    [SerializeField]Animation screenCoverfade;
    Animation fadeIn, fadeOut;

    // Start is called before the first frame update
    void Start()
    {
        h = GetComponent<Health>();

    }

    // Update is called once per frame
    void Update()
    {
        if (h.dead)
        {
            StartCoroutine(delayReturn());

        }
    }

    IEnumerator delayReturn()
    {
        float timer = 0;
        //screenCoverfade.Play();
        while (timer < delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        transform.position = spawn.position;
        h.revive();
        //screenCoverfade.Play();
    }
}
