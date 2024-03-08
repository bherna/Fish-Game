using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Enemy : MonoBehaviour
{
   
   
   //enemy prefabs
   [SerializeField] GameObject enemy;

    private List<GameObject> enemyList;


    //spawning
    private int secs_till_next_enemyWave = 10;

    private float curr_sec = 0f;

    private bool spawning = true;


    

    private void Start() {
        
        enemyList = new List<GameObject>();
    }



    private void Update() {
        
        //if the waves are still comming in
        //spawn waves
        if(spawning){

            curr_sec += Time.deltaTime;

            if(curr_sec >= secs_till_next_enemyWave){

                SpawnWave();
                curr_sec = 0;
            }
        }
    }


    private void SpawnWave(){

        Instantiate(enemy, new Vector2(0, 0), Quaternion.identity); 

    }

}
