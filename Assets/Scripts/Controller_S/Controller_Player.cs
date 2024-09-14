using System;
using UnityEngine;
using UnityEngine.UI;

public class Controller_Player : MonoBehaviour
{
    
    [SerializeField] ParticleSystem Gun_particle;

    //used for getting mouse position (what is our target z axis) (is in the bg-level gameobject)
    [SerializeField] Transform targetZ;

    public Vector3 mousePos;

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


    private void Start() {

    }

    private void Update() {

        //get mouse pos
        var screenPos = Input.mousePosition;
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, targetZ.position - Camera.main.transform.position);
        mousePos = Camera.main.ScreenToWorldPoint(screenPos); 
        
        //move self there
        transform.position = new Vector2(mousePos.x, mousePos.y);
    }


    public void Upgrade_gunPower() {
        gunPower += 1;
    }

    public int Get_GunDamage(){
        return gunPower;
    }


    public void Run_GunParticle(){
        Instantiate(Gun_particle, mousePos, Quaternion.identity);
    }






}
