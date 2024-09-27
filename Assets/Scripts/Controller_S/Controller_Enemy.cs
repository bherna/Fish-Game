using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class Controller_Enemy : MonoBehaviour
{
   



    //spawning
    private float curr_sec = 0f;
    private bool keepSpawning = true;
    private float preAnnouncerTime = 3f;
    private bool currently_in_wave = false; //used in keeping the wave from spawning multiple times (from coroutine being used)
    private bool startWaves = false;


    //bot ui
    [SerializeField] GameObject annoucement_ui;
    [SerializeField] TextMeshProUGUI ui_text;



    //Enemy_wave script variables to hold (current wave to hold)
    [SerializeField] private Enemy_Waves enemy_Waves;
    private int secs_till_next_enemyWave = 0; 
    private int currWaveIndex = 0;

    //current amount of enemies on screen
    private int enemiesOnScreen = 0;

    //should we loop the enemy waves
    [SerializeField] bool loop = false;


    //singleton this class
    public static Controller_Enemy instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }
    

    private void Start() {
        
        //disable annoumcent (just in case)
        annoucement_ui.SetActive(false);

        //update wave matrix
        //also make sure we have a matrix to work with
        try{
            secs_till_next_enemyWave = enemy_Waves.Index_GetTimeTillSpawn(currWaveIndex);

        }catch(IndexOutOfRangeException e){
            Debug.LogError(e);
            Debug.Log("No Enemy waves to anticipate :( In Enemy_Waves, add new waves for enemy controller to spawn.");
            keepSpawning = false;
        }
        
        

    
    }

    public void StartWaves(){
        startWaves = true;
    }



    private void Update() {

        if(!startWaves){
            return;
        }
        
        //if the waves are still comming in
        //spawn waves
        if(keepSpawning && !currently_in_wave){

            //timer update
            curr_sec += Time.deltaTime;

            //should we spawn wave
            if(curr_sec >= secs_till_next_enemyWave){

                //spawn wave + post wave stats
                currently_in_wave = true; // turn off the show we spawn command (since we are spawning)
                IEnumerator coroutine = SpawnWave(preAnnouncerTime);
                StartCoroutine(coroutine);
                
            }
        }
    }


    private IEnumerator SpawnWave(float waitTIme){

        //annouce (they are comming)
        ui_text.text = "Enemies are coming!";
        annoucement_ui.SetActive(true);
        
        //wait
        yield return new WaitForSeconds(waitTIme);

        //for each enemy in our current wave
        enemy_Waves.Index_GetWave(currWaveIndex).ForEach(delegate(GameObject enemy)
        {
            //spawn enemy
            var randSpot = RandomTankSpawnSpot();
            var temp = Instantiate(enemy, randSpot, Quaternion.identity); 
        });

        //announce (current enemies count)
        enemiesOnScreen = enemy_Waves.Index_GetWave(currWaveIndex).Count;
        ui_text.text = "Enemies are here: " + enemiesOnScreen.ToString();


        //get next wave set up
        NextWaveSetup();
        
        
    }

    //set up for the next wave, if we have a next wave we set our variables
    //else if we should restart from begining
    //else no more waves
    private void NextWaveSetup(){

        //set current second to 0
        curr_sec = 0;

        //update wave index to next wave
        currWaveIndex += 1;

        //is there a next wave we can spawn?
        if (currWaveIndex < enemy_Waves.GetLength()){
            //set our new seconds till spawn next wave
            secs_till_next_enemyWave = enemy_Waves.Index_GetTimeTillSpawn(currWaveIndex);
        }
        //if not, should we restart?
        else if(loop) {
            //go to index 0 and get timer length again
            currWaveIndex = 0;
            secs_till_next_enemyWave = enemy_Waves.Index_GetTimeTillSpawn(currWaveIndex);

        }
        //else no more waves
        else{
            //stop spawning
            keepSpawning = false;
            Debug.Log("End of enemy waves\n");
        }
    }


    private Vector2 RandomTankSpawnSpot(){

        var spawnDem = TankCollision.instance.GetTankSpawnArea();
        
        var idleTarget = new Vector3(
                Random.Range(spawnDem.Item1, spawnDem.Item2),
                Random.Range(spawnDem.Item3, spawnDem.Item4),
                transform.position.z
            );

        return idleTarget;
    }


    //everytime an enemy dies in tank, run this
    public void CloserToWaveEnded(){

        //decrement 
        enemiesOnScreen -= 1;

        //update announcement
        ui_text.text = "Enemies are here: " + enemiesOnScreen.ToString();

        //check if no more enemies
        if(enemiesOnScreen <= 0){

            //disable annoucement
            annoucement_ui.SetActive(false);
            //start counting again
            currently_in_wave = false;
            //tell controller pets that enemy wave is over
            Controller_Pets.instance.Annoucement_NoMoreEnemies();
        }
        
    }
}
