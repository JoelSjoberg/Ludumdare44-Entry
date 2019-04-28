using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audioManager : MonoBehaviour
{

    public static GameObject instance;
    public Sound[] soundList;

    private static Sound[] sounds;

    // Use this for initialization
    void Awake()
    {
        /*
        // Singleton
        if (instance == null) instance = gameObject;
        else
        {

            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        */

        foreach (Sound s in soundList)

        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.looping;
            s.source.playOnAwake = false;

        }

        

        sounds = soundList;
    }

    private void Start()
    {
        play_muted("viola");
        play_muted("baroche");
        play_muted("bass");
        play("drums");
    }
    public void play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("The sound " + name + " was not found, check the name definition or append the sound to the audio manager");
        }
        else s.play();
    }

    public void play_muted(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("The sound " + name + " was not found, check the name definition or append the sound to the audio manager");
        }
        else
        {
            s.source.volume = 0;
            s.play();
        }
    }

    public void SoundfadeOut(string name, float goal = 0)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("The sound " + name + " was not found, check the name definition or append the sound to the audio manager");
        }
        else StartCoroutine(fadeOut(s, goal));
    }

    public void SoundFadeIn(string name, float goal = 1)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("The sound " + name + " was not found, check the name definition or append the sound to the audio manager");
        }
        else StartCoroutine(fadeIn(s, goal));
    }


    public void stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("The sound " + name + " was not found, check the name definition or append the sound to the audio manager");
        }
        else s.stop();
    }

    public void fadeAndStop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("The sound " + name + " was not found, check the name definition or append the sound to the audio manager");
        }
        else StartCoroutine(fadeOutAndStop(s));
    }

    // static methods, use these instead!
    public static void playSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("The sound " + name + " was not found, check the name definition or append the sound to the audio manager");
        }
        else s.play();
    }

    public static void stopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("The sound " + name + " was not found, check the name definition or append the sound to the audio manager");
        }
        else s.stop();
    }


    IEnumerator fadeOutAndStop(Sound s)
    {
        while (s.source.volume > 0)
        {
            s.source.volume -= 1f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        s.stop();

    }
    IEnumerator fadeIn(Sound s, float goal)
    {
        while (s.source.volume < goal)
        {
            s.source.volume += 0.5f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator fadeOut(Sound s, float goal)
    {
        while (s.source.volume > goal)
        {
            s.source.volume -= 1f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


    }
}
