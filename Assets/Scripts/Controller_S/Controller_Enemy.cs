using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Enemy : MonoBehaviour
{
   

    

    //tank dem
    private float tank_xLower;
    private float tank_xUpper;
    private float tank_yLower;
    private float tank_yUpper;



    //spawning
    [SerializeField]  int secs_till_next_enemyWave;

    private float curr_sec = 0f;

    private bool spawning = true;


    //controller referencess
    [SerializeField] Controller_Fish controller_Fish;



    //list for spawning
    private EnemyWave[] wave_mat; 
    private int currWaveIndex = 0;
    

    private void Start() {
        
        //update tank demension for spawning
        (tank_xLower, tank_xUpper, tank_yLower, tank_yUpper) = TankCollision.instance.GetTankSpawnArea();

        //update wave mat
        wave_mat = GetComponent<Enemy_Waves>().Get_WaveMat();

        //set seconds for wave spawn
        secs_till_next_enemyWave = wave_mat[currWaveIndex].secTillWaveSpawn;
        
    }



    private void Update() {
        
        //if the waves are still comming in
        //spawn waves
        if(spawning){

            //timer
            curr_sec += Time.deltaTime;

            //should we spawn wave
            if(curr_sec >= secs_till_next_enemyWave){

                //spawn wave 
                SpawnWave();
                //set current second to 0
                curr_sec = 0;

                //update wave index to next wave
                currWaveIndex += 1;

                //are we now done with spawning waves?
                if(!(currWaveIndex < wave_mat.Length)){

                    //stop spawning
                    spawning = false;
                    Debug.Log("End of enemy waves");
                }
                else{
                    //set our new seconds till spawn next wave
                    secs_till_next_enemyWave = wave_mat[currWaveIndex].secTillWaveSpawn;
                }

                

            }
        }
    }


    private void SpawnWave(){

        //for each enemy in our current wave
        wave_mat[currWaveIndex].wave.ForEach(delegate(GameObject enemy)
        {
            //Debug.Log(enemy.name);
            
            var randSpot = NewRandomTankSpot();
            var temp = Instantiate(enemy, randSpot, Quaternion.identity); 
            temp.GetComponent<Enemy>().SetController_Enemy(this);
            temp.GetComponent<Enemy>().SetTargetFish(GetRandomFish());
        });
        
    }





    public Transform GetRandomFish(){

        return controller_Fish.GetRandomFish();
    }


    


    private Vector2 NewRandomTankSpot(){
        var idleTarget = new Vector2(
                Random.Range(tank_xLower, tank_xUpper),
                Random.Range(tank_yLower, tank_yUpper)
            );

        return idleTarget;
    }

}
