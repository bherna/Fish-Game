using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravelSpawn : MonoBehaviour
{
    
    //reference to the gravel spawn area
    [SerializeField] BoxCollider2D spawnRange;

    //gravel pebble to spawn
    [SerializeField] GameObject pebble;
    
    //scale for how many pebbles in scene 
    float pebble_amount = 2.5f;

    //size of each pebble (from original)
    [SerializeField] float pebble_size = 1;

    //spawn dimensions
    float spawn_xLower;
    float spawn_yUpper;


    //gradient for colors scale(1-100)
    private int brown_s;
    private int green_s;
    private int white_s;
    private int tan_s;


    private void Awake() {
        
        GetGravelDimensions();
        SpawnGravel();
    }


    private void GetGravelDimensions(){

        var w = spawnRange.size.x;
        var h = spawnRange.size.y;

        var tank_pos = spawnRange.offset;

        spawn_xLower = tank_pos.x - w/2;
        spawn_yUpper = tank_pos.y + h/2;
    }

    private void SpawnGravel(){


        //assuming pebble size is 1 unit
        //var pebble_length = 1;

        //pebble x range
        float row_count = spawnRange.size.x;
        float col_count = spawnRange.size.y;

        //spawn pebbles in box size
        for (float row = 0; row < row_count; row += 1){
            for (float col = 0; col < col_count; col += 1){
                Instantiate(pebble,
                new Vector2(spawn_xLower + row, spawn_yUpper - col),
                Quaternion.identity);

            }
        }



    }


    private void GetColor(){
        
    }

}
