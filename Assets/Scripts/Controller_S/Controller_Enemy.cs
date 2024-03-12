using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Enemy : MonoBehaviour
{
   

    //tank collider
    [SerializeField] GameObject tankColl;

    //tank dem
    private float tank_xLower;
    private float tank_xUpper;
    private float tank_yLower;
    private float tank_yUpper;



    //spawning
    [SerializeField]  int secs_till_next_enemyWave = 10;

    private float curr_sec = 0f;

    private bool spawning = true;


    //controller referencess
    [SerializeField] Controller_Fish controller_Fish;



    //list for spawning
    [SerializeField] List<List<GameObject>> wave_mat; 
    private int currWaveIndex = 0;
    

    private void Start() {
        
        //update tank demension for spawning
        GetTankDem();

        //update wave mat
        wave_mat = GetComponent<Enemy_Waves>().Get_WaveMat();
        
    }



    private void Update() {
        
        //if the waves are still comming in
        //spawn waves
        if(spawning){

            curr_sec += Time.deltaTime;

            if(curr_sec >= secs_till_next_enemyWave){

                SpawnWave();
                curr_sec = 0;

                currWaveIndex += 1;
                if(currWaveIndex >= wave_mat.Count){
                    spawning = false;
                    Debug.Log("End of enemy waves");
                }

            }
        }
    }


    private void SpawnWave(){

        /*
        for(int i = 0; i < wave_mat[currWaveIndex]; i++){

            var randSpot = NewRandomTankSpot();
            var temp = Instantiate(enemy1, randSpot, Quaternion.identity); 
            temp.GetComponent<Enemy>().SetController_Enemy(this);
            temp.GetComponent<Enemy>().SetTargetFish(GetRandomFish());

        }
        */

        wave_mat[currWaveIndex].ForEach(delegate(GameObject enemy)
        {
            Debug.Log(enemy.name);
        });
        
    }





    public Transform GetRandomFish(){

        return controller_Fish.GetRandomFish();
    }


    public void GetTankDem(){

        var tank_size = tankColl.GetComponent<BoxCollider2D>().size;
        var w = tank_size.x;
        var h = tank_size.y;

        var tank_pos = tankColl.transform.position;

        tank_xLower = tank_pos.x - w/2;
        tank_xUpper = tank_pos.x + w/2;

        tank_yLower = tank_pos.y - h/2;
        tank_yUpper = tank_pos.y + h/2;

       
    }


    private Vector2 NewRandomTankSpot(){
        var idleTarget = new Vector2(
                Random.Range(tank_xLower, tank_xUpper),
                Random.Range(tank_yLower, tank_yUpper)
            );

        return idleTarget;
    }

}
