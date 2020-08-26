using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        else if (instance != null)
        {
            Destroy(gameObject);    //gameObject is a local variable of type GameObject which is inherited from Component.                                   //It allows one to access the instance of the GameObject to which this component is attached.
        }

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
        }
    }

    public void Play (string name)
    {
        Sound sound = System.Array.Find(sounds, s => s.name == name);
        sound.source.Play();
    }
}
