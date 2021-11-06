using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip audioClip;
        [Range(0f,1f)]
        public float volume;
        [Range(0.1f,3.0f)]
        public float pitch;

        public bool playOnAwake = false;
        public bool loop;
        public AudioMixerGroup mixerOutput;
        
        [HideInInspector]
        public AudioSource audioSource;
    }

    public Sound[] sounds;
    private List<AudioSource> _sources = new List<AudioSource>();
    
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        { 
            _instance = this;
        }

        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.audioClip;
            s.audioSource.volume = s.volume;
            s.audioSource.playOnAwake = s.playOnAwake;
            s.audioSource.pitch = s.pitch;
            s.audioSource.outputAudioMixerGroup = s.mixerOutput;
            s.audioSource.loop = s.loop;

        }
    }

    public void Play(int index)
    {
        sounds[index].audioSource.Play();
    }
    public void Stop(int index)
    {
        sounds[index].audioSource.Stop();
    }

}
