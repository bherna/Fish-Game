using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankCollision : MonoBehaviour
{

    //reference to self (global)
    public static TankCollision instance {get; private set; }

    //reference to tank area collision box
    [SerializeField] BoxCollider2D swimRange;

    //reference to spawn collision box
    [SerializeField] BoxCollider2D spawnRange;

    //spawn range
    float spawn_xLower;
    float spawn_xUpper;
    float spawn_yLower;
    float spawn_yUpper;

    //swim range
    float swim_xLower;
    float swim_xUpper;
    float swim_yLower;
    float swim_yUpper;
       
    private void Awake() {
        
        //delete duplicate of this instance
        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }


        GetSwimDimensions();
        GetSpawnDimensions();
    }

    private void GetSwimDimensions(){

        var w = swimRange.size.x;
        var h = swimRange.size.y;

        var tank_pos = swimRange.offset;

        swim_xLower = tank_pos.x - w/2;
        swim_xUpper = tank_pos.x + w/2;

        swim_yLower = tank_pos.y - h/2;
        swim_yUpper = tank_pos.y + h/2;

    }

    private void GetSpawnDimensions(){

        var w = spawnRange.size.x;
        var h = spawnRange.size.y;

        var tank_pos = spawnRange.offset;

        spawn_xLower = tank_pos.x - w/2;
        spawn_xUpper = tank_pos.x + w/2;

        spawn_yLower = tank_pos.y - h/2;
        spawn_yUpper = tank_pos.y + h/2;

    }



    public (float, float, float, float) GetTankSwimSpawnArea(){
            return (swim_xLower, swim_xUpper, swim_yLower, swim_yUpper);
    } 

    public (float, float, float, float) GetTankSpawnArea(){
        return (spawn_xLower, spawn_xUpper, spawn_yLower, spawn_yUpper);
    } 
    

    
}
