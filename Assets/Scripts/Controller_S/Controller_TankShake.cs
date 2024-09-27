using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_TankShake : MonoBehaviour
{

    [SerializeField] ParticleSystem shake_bubbles;

    private float intensity = 0.7f;
    private int length = 4;
    private Vector3 startPos;

    
    //singleton this class
    public static Controller_TankShake instance {get; private set; }
    private void Awake() {
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }


    private void Start() {
        
        //save current start position
        startPos = transform.position;
    }




    //everytime someone calls this function
    //shake the screen
    public void ShakeTank(){

        StartCoroutine(I_ShakeTank());
    }
    private IEnumerator I_ShakeTank(){

        //make shake bubbles
        Instantiate(shake_bubbles, new Vector3(0,0,0), Quaternion.identity);

        //shake
        for(int i = 0; i < length; i++) {

            var randPos = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), transform.position.z);
            transform.localPosition = randPos;
            yield return new WaitForSeconds(0.1f);
        }

        //reset position + bubbles
        transform.position = startPos;

    }


}
