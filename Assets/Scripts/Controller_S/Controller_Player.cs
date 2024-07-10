using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
    
    [SerializeField] ParticleSystem Gun_particle;

    //used for getting mouse position (what is our target z axis) (is in the bg-level gameobject)
    [SerializeField] Transform targetZ;


    //player stats
    //gun stat
    private int gunPower = 1;





    public static Controller_Player instance {get; private set; }

    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    } 

    private void Update() {
        
        if(Input.GetMouseButtonDown(0)){

            var screenPos = Input.mousePosition;
            screenPos.z = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
            var mousePos = Camera.main.ScreenToWorldPoint(screenPos); 
            Instantiate(Gun_particle, mousePos, Quaternion.identity);
        }
    }




    public void Upgrade_gunPower() {
        gunPower += 5;
    }

    public int Get_GunDamage(){
        return gunPower;
    }
}
