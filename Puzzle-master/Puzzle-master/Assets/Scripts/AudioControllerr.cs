using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioControllerr : MonoBehaviour
{
    AudioManagerr audioManager;
    public int sound;

    private string turnedOFF="MUSIC OFF", turnedON="MUSIC ON";
    public TextMeshProUGUI[] soundButtonTexts;

    private void Start()
    {
        audioManager = GetComponent<AudioManagerr>();

        sound = PlayerPrefs.GetInt("sound", 1);//0 means sound off, 1 means sound on
        setProperSoundButtonTexts();
        audioManager.fetchAudioSourceStates(!isSoundOFF());
        audioManager.Play("Background");
    }
    public bool isSoundOFF()//returns false if sound ON, and vice versa
    {
        return (sound == 1) ? false : true;
    }

    public void setProperSoundButtonTexts()
    {
        sound = PlayerPrefs.GetInt("sound", 1);
        for (int i = 0; i < soundButtonTexts.Length; i++)
        {
            soundButtonTexts[i].text = (isSoundOFF()) ? turnedOFF : turnedON;
        }
    }
}
