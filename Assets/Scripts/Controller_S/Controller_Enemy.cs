using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller_Enemy : MonoBehaviour
{
   

    

    //tank dem
    private float tank_xLower;
    private float tank_xUpper;
    private float tank_yLower;
    private float tank_yUpper;



    //spawning
    private float curr_sec = 0f;
    private bool keepSpawning = true;
    private float preAnnouncerTime = 3f;
    private bool currently_in_wave = false; //used in keeping the wave from spawning multiple times (from coroutine being used)


    //controller referencess
    [SerializeField] Controller_Fish controller_Fish;
    [SerializeField] GameObject annoucement_ui;



    //Enemy_wave script variables to hold (current wave to hold)
    [SerializeField] private Enemy_Waves enemy_Waves;
    private List<GameObject> currentEnemyWaveList;
    private int secs_till_next_enemyWave = 0; 
    private int currWaveIndex = 0;

    //current amount of enemies on screen
    private int enemiesOnScreen = 0;
    

    private void Start() {
        
        //update tank demension for spawning
        (tank_xLower, tank_xUpper, tank_yLower, tank_yUpper) = TankCollision.instance.GetTankSpawnArea();

        //update wave mat
        currentEnemyWaveList = enemy_Waves.Index_GetWave(currWaveIndex);

        //set seconds for wave spawn
        secs_till_next_enemyWave = enemy_Waves.Index_GetTimeTillSpawn(currWaveIndex);

        //disable annoumcent (just in case)
        annoucement_ui.SetActive(false);
    }



    private void Update() {
        
        //if the waves are still comming in
        //spawn waves
        if(keepSpawning && !currently_in_wave){

            //timer update
            curr_sec += Time.deltaTime;

            //should we spawn wave
            if(curr_sec >= secs_till_next_enemyWave){

                //spawn wave + post wave stats
                currently_in_wave = true; // turn off the show we spawn command (since we are spawning)
                enemiesOnScreen = enemy_Waves.Index_GetWave(currWaveIndex).Count;
                IEnumerator coroutine = SpawnWave(preAnnouncerTime);
                StartCoroutine(coroutine);
                
            }
        }
    }


    private IEnumerator SpawnWave(float waitTIme){

        //annouce 
        annoucement_ui.SetActive(true);
        
        //wait
        yield return new WaitForSeconds(waitTIme);

        //for each enemy in our current wave
        enemy_Waves.Index_GetWave(currWaveIndex).ForEach(delegate(GameObject enemy)
        {
            
            var randSpot = NewRandomTankSpot();
            var temp = Instantiate(enemy, randSpot, Quaternion.identity); 
            temp.GetComponent<Enemy>().SetController_Enemy(this);
        });


        //set current second to 0
        curr_sec = 0;

        //update wave index to next wave
        currWaveIndex += 1;

        //are we now done with spawning waves?
        try{
            //try checking if next wave exists
            enemy_Waves.Index_GetWave(currWaveIndex);
            //if yes then...
            //set our new seconds till spawn next wave
            secs_till_next_enemyWave = enemy_Waves.Index_GetTimeTillSpawn(currWaveIndex);
        }
        catch(IndexOutOfRangeException ex){
            //else
            //stop spawning
            keepSpawning = false;
            Debug.Log("End of enemy waves\n" + ex.Message);
        }
        
        
    }

    
    //enemy in tank calls this when no new enemies are on screen
    public Transform GetRandomFish(){

        return controller_Fish.GetRandomFish();
    }

    private Vector2 NewRandomTankSpot(){
        var idleTarget = new Vector3(
                Random.Range(tank_xLower, tank_xUpper),
                Random.Range(tank_yLower, tank_yUpper),
                transform.position.z
            );

        return idleTarget;
    }


    //everytime an enemy dies in tank, run this
    public void CloserToWaveEnded(){
        enemiesOnScreen -= 1;

        if(enemiesOnScreen <= 0){

            //disable annoucement
            annoucement_ui.SetActive(false);
            //start counting again
            currently_in_wave = false;
        }
        
    }
}
