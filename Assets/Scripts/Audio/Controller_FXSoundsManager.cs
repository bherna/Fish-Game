using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_FXSoundsManager : MonoBehaviour
{


    //gameobject we are instancing
    //doesn't need to be gameobject (we can just get the component, hm)
    [SerializeField] private AudioSource soundFXObject;


    //singleton settup
    public static Controller_FXSoundsManager instance {get; private set; }
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



    //this function is used in creating all the little sound effects that happen through out the game,
    //it creates an instance audio that
    //plays its audio, then after finishing
    //destroy self
    //for parameters: 
    //      volume - its just used in layer sound effects with each other, not true percentage value cause thats controlled by  soundmixermanager
    //               we dont need to multply or nothing since thats already done with the fx volume control (soundMixerManager)
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volumePercent, float pitch)
    {

        //spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assing the audio clip
        audioSource.clip = audioClip;

        //assign pitch
        audioSource.pitch = pitch;

        //assign volume
        audioSource.volume = volumePercent;

        //play sound
        audioSource.Play();

        // get length of sound fx clip
        float cliplength = audioSource.clip.length;

        //destroy the clip once done playing
        Destroy(audioSource.gameObject, cliplength);

    }


    //play a random audio clip, given a list of them,
    public void PlayRandomSoundFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume){

        //random index
        int rand = Random.Range(0, audioClip.Length);

        //spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assing the audio clip
        audioSource.clip = audioClip[rand];

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        // get length of sound fx clip
        float cliplength = audioSource.clip.length;

        //destroy the clip once done playing
        Destroy(audioSource.gameObject, cliplength);

    }
}
