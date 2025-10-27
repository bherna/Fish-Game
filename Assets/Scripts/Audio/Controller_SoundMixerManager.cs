using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum VolumeType {Master, Music, FX, Null}; //should never be null


public class Controller_SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;


    private Dictionary<VolumeType, string> volumeTypeStrings = new Dictionary<VolumeType, string>
    {
        {VolumeType.Master, "MasterVolume"},
        {VolumeType.Music, "MusicVolume"},
        {VolumeType.FX, "SoundFXVolume"}
    };
    




    //singleton settup
    public static Controller_SoundMixerManager instance { get; private set; }
    void Awake()
    {

        //delete duplicate of this instance

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;

    public void LowPass(bool wePausing)
    {
        if (wePausing)
        {
            paused.TransitionTo(0.001f);
        }
        else
        {
            unpaused.TransitionTo(0.001f);
        }
    }



    //these are called from the settings page, used in updating our volumes
    public void SetNewVolume(VolumeType type, float newPercent)
    {
        //volume being a linear graph kinda sucks i've heard, so originally we had a logbase10(x) *20 graph setup but that doesn work
        //any more for some reason, so we moved on to just making it quadratic
        int newVolume = (int)((-70) * Mathf.Pow(newPercent - 1, 2) - 10); //keep as whole number for clarity
        
        audioMixer.SetFloat(volumeTypeStrings[type], newVolume);
        Debug.Log(string.Format("New {0} at {1:0%} is {2} hrtz", volumeTypeStrings[type], newPercent, newVolume));
    }







    

}
