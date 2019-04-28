using UnityEngine;

[System.Serializable]
public class Sound
{

    public string name;

    public AudioClip clip;
    [HideInInspector] public AudioSource source;

    [Range(0, 10)] public float volume = 1;
    [Range(0, 5)] public float pitch = 1;
    public bool looping;

    public void play()
    {
        source.Play();
    }

    public void stop()
    {
        source.Stop();
    }

}
