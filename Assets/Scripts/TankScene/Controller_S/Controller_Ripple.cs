
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


    public void CreateRipple(Vector2 init_pos){

        Vector3 newPos = new Vector3(init_pos.x, init_pos.y, transform.TransformPoint(transform.position).z);
        var newRipple = Instantiate(ripple_ps, newPos, Quaternion.identity);

    }


}
