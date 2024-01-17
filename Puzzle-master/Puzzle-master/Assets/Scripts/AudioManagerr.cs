using System;
using UnityEngine;

public class AudioManagerr : MonoBehaviour
{
    public Sound[] sounds;

    [SerializeField]
    private AudioSource[] audioSources;

    AudioControllerr audioControllerr;

    bool fSFXcontrol = false;

    private void Awake()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;

            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;

            sounds[i].source.loop = sounds[i].loop;
            sounds[i].source.playOnAwake = sounds[i].playOnAwake;

            audioSources[i] = sounds[i].source;
        }
    }

    private void Start()
    {
        audioControllerr = GetComponent<AudioControllerr>();
    }

    public void Play(string name)
    {//to play sound

        Sound s = Array.Find(sounds, sound => sound.clipName == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound : {name} does not exist!");
            return;
        }

        s.source.Play();
    }

    public void Mute(string name)
    {//to mute sound

        Sound s = Array.Find(sounds, sound => sound.clipName == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound : {name} does not exist!");
            return;
        }

        s.source.mute = true;
    }

    public void UnMute(string name)
    {//to mute sound

        if (audioControllerr.isSoundOFF() && !name.Equals("Background"))//sound was turned off by user
            return;

        Sound s = Array.Find(sounds, sound => sound.clipName == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound : {name} does not exist!");
            return;
        }

        s.source.mute = false;
    }

    public void Stop(string name)
    {//stop any music by name

        Sound s = Array.Find(sounds, sound => sound.clipName == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound : {name} does not exist!");
            return;
        }

        s.source.Stop();
    }

    public void muteAndUnmuteAllSounds()
    {//f==false means unmute, f==true means mute
        
        if (audioControllerr.sound == 1)
        {
            fSFXcontrol = false;//turn off voice
        }
        else
        {
            fSFXcontrol = true;//turn on voice
        }

        fetchAudioSourceStates(fSFXcontrol);

        PlayerPrefs.SetInt("sound", fSFXcontrol ? 1:0);

        audioControllerr.setProperSoundButtonTexts();
    }

    public void fetchAudioSourceStates(bool f)//f true will be false (non check), f false will be check
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i].mute = !f;
        }
    }

    public bool isPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.clipName == name);

        if (s == null)
        {
            Debug.LogWarning($"Sound : {name} does not exist!");
            return false;
        }

        return s.source.isPlaying;
    }
}
