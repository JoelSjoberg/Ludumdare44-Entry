using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class onPlayerDeath : MonoBehaviour
{
    [SerializeField]Transform spawn;
    Health h;
    [SerializeField]float delay = 2f;
    [SerializeField]Image screenCoverfade;

    bool coroutine_running = false;
    // Start is called before the first frame update
    void Start()
    {
        h = GetComponent<Health>();
        StartCoroutine(FadeTo(0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        if (h.dead && !coroutine_running)
        {
            StartCoroutine(delayReturn());
            coroutine_running = true;
        }
    }

    IEnumerator delayReturn()
    {
        float timer = 0;
        StartCoroutine(FadeTo(255, 1));
        while (timer < delay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        print("returned to start");
        transform.position = spawn.position;
        h.revive();
        coroutine_running = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        StartCoroutine(FadeTo(0, 1));
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = screenCoverfade.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, aValue, t));
            screenCoverfade.color = newColor;
            yield return null;
        }
    }
}
