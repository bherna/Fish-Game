using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using System.Collections.Generic;


public class Controller_Enemy : MonoBehaviour
{
   

    //spawning
    private float curr_sec = 0f;
    private bool keepSpawning = true;
    private float preAnnouncerTime = 3f;
    public bool currently_in_wave {get; private set;} = false; //used in keeping the wave from spawning multiple times (from coroutine being used)
    private bool startWaves = false;


    //annoucement ui
    [SerializeField] GameObject annoucement_ui;
    [SerializeField] TextMeshProUGUI ui_text;


    //Enemy_wave script variables to hold (current wave to hold)
    private Tank_EnemyWaves tank_EnemyWaves
    ; 
    private int secs_till_next_enemyWave = 0; 
    private int currWaveIndex = 0;

    //current amount of enemies on screen
    private int enemiesOnScreen = 0;
    private List<GameObject> enemies;



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

        //read in enemy wave json file
        tank_EnemyWaves = LocalLevelVariables.GetTank_EnemyWaves();

        //check if we have waves to spawn first
        if(tank_EnemyWaves.TotalWavesCount() > 0){
            //update our seconds till enemy wave
            secs_till_next_enemyWave = tank_EnemyWaves.SecsTillSpawn(currWaveIndex);
        }
        else{
            Debug.Log("We have no enemy waves to run :p");
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

                //only run once
                currently_in_wave = true; 
                //check if our wave is size 0
                if(tank_EnemyWaves.GetWave(currWaveIndex).Length > 0){
                    //spawn wave normally
                    StartCoroutine(SpawnWave_tutorial());
                }
                else{
                    Debug.Log("skipped wave");
                    //we are just using this as a countdown for next wave
                    NextWaveSetup();
                    CloserToWaveEnded();
                }
                
            }
        }
    }

    //two parts to this function
    //first part: we announce to player that enemies are going to spawn soon
    //              after a few seconds
    //second part: update annoucment to say how many enemies spawned this time
    //              spawn all enemies
    //              tell controller_pets that we started a wave, 
    //              then we start getting ready for next wave
    private IEnumerator SpawnWave_tutorial(){

        //annouce (they are comming)
        ui_text.text = "Enemies are coming!";
        annoucement_ui.SetActive(true);

        //announce to controlle tutorial
        TutorialReaderParent.instance.EnemyWaveStarting();
        
        //wait atleast once,
        //keep waiting if :     - player is in tutorial and is currently reading the dialogue (so not waiting)
        //                      - we are still in the tutorial
        do{
            yield return new WaitForSeconds(preAnnouncerTime);
        }
        while(!TutorialReaderParent.instance.waiting && Controller_Tutorial.instance.tutorial_active);

        //for each enemy in our current wave
        enemies = new List<GameObject>();
        foreach(string enemyName in tank_EnemyWaves.GetWave(currWaveIndex))
        {
            //spawn enemy
            var randSpot = RandomTankSpawnSpot();
            enemies.Add(Instantiate(Resources.Load("Enemies/" + "Enemy_"+enemyName.ToString()) as GameObject, randSpot, Quaternion.identity)); 
        }

        //announce (current enemies count)
        enemiesOnScreen = tank_EnemyWaves.GetWave(currWaveIndex).Length;
        ui_text.text = "Enemies are here: " + enemiesOnScreen.ToString();

        //announce to controller_pets we just spawned enemies
        Controller_Pets.instance.Annoucement_Init(Event_Type.enemyWave, gameObject);


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
        if (currWaveIndex < tank_EnemyWaves.TotalWavesCount()){
            //set our new seconds till spawn next wave
            secs_till_next_enemyWave = tank_EnemyWaves.SecsTillSpawn(currWaveIndex);
        }
        //if not, should we restart?
        else if(tank_EnemyWaves.loop) {
            //go to index 0 and get timer length again
            currWaveIndex = 0;
            secs_till_next_enemyWave = tank_EnemyWaves.SecsTillSpawn(currWaveIndex);

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
            Controller_Pets.instance.Annoucement_EndIt(Event_Type.enemyWave);
            //tell tutorial we finished aswell
            TutorialReaderParent.instance.EnemyWaveOver();
        }
        
    }


    public GameObject GetEnemyAtIndex(int index){
        return enemies[index];
    }

}
