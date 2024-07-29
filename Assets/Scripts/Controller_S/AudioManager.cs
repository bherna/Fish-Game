using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    
    //gameobject we are instancing
    //doesn't need to be gameobject (we can just get the component, hm)
    [SerializeField] private AudioSource soundFXObject;


    //static variable for fish coin value
    public static AudioManager instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }



    //instance audio
    //play audio
    //destroy self
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume){

        //spawn in gameobject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assing the audio clip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        // get length of sound fx clip
        float cliplength = audioSource.clip.length;

        //destroy the clip once done playing
        Destroy(audioSource.gameObject, cliplength);

    }


    //play a random audio clip
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
