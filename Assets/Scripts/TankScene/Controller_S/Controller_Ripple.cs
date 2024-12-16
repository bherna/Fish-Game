
using System;
using Unity.Mathematics;
using UnityEngine;


//all this class does for now is instantiate new ripple_ particle systems (that should self destroy)
//this object this is attached to is the ripple_water object that already controls the ripple shader 
//since the ps needs to touch the collider of this object, might as well make them spawn here
public class Controller_Ripple : MonoBehaviour
{

    [SerializeField] GameObject ripple_ps; //particle system is attached to this obj


    //since other classes are going to cause ripples to be made, they can call this class
    //single ton this class
    public static Controller_Ripple instance {get; private set; }

    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }


    //give a world position and this function will create a ripple on the correct position to ripple with water
    public void CreateRipple(Vector2 init_pos){

        Vector3 newPos = new Vector3(init_pos.x, init_pos.y, transform.TransformPoint(transform.position).z);
        var newRipple = Instantiate(ripple_ps, newPos, Quaternion.identity);
    }


    //use this to get the z of this object attach to in global space, since apparently we need a way
    //to ignore transparents, this is the only way i can think of that works for me
    //just get the Z value, and add -0.1 to the transparent object so it renders in front of this shit 
    public float GetZ(){

        return transform.TransformPoint(transform.position).z;
    }
}
