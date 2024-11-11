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

    //should we loop the enemy waves
    [SerializeField] bool loop = false;

    //Enemy_wave script variables to hold (current wave to hold)
    private Tank_EnemyWaves enemyWaves; 
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


    public static string LoadResourceTextfile(string path)
    {

        string filePath = "Enemies/TankWaves/" + path.Replace(".json", "");

        TextAsset targetFile = Resources.Load<TextAsset>(filePath);

        return targetFile.text;
    }


    

    private void Start() {
        
        //disable annoumcent (just in case)
        annoucement_ui.SetActive(false);

        //read in enemy wave json file
        Tank_EnemyWaves temp = JsonUtility.FromJson<Tank_EnemyWaves>(LoadResourceTextfile("level_1-1"));
        enemyWaves = new Tank_EnemyWaves(temp);

        //update wave matrix
        //also make sure we have a matrix to work with
        try{
            secs_till_next_enemyWave = enemyWaves.SecsTillSpawn(currWaveIndex);

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

                //spawn wave + post wave stats + announce to controller_pets
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
        enemies = new List<GameObject>();
        enemyWaves.GetWave(currWaveIndex).ForEach(delegate(string enemyName)
        {
            //spawn enemy
            var randSpot = RandomTankSpawnSpot();
            enemies.Add(Instantiate(Resources.Load("Enemies/EnemyPrefabs/" + "Enemy_"+enemyName.ToString()) as GameObject, randSpot, Quaternion.identity)); 
        });

        //announce (current enemies count)
        enemiesOnScreen = enemyWaves.GetWave(currWaveIndex).Count;
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
        if (currWaveIndex < enemyWaves.TotalWavesCount()){
            //set our new seconds till spawn next wave
            secs_till_next_enemyWave = enemyWaves.SecsTillSpawn(currWaveIndex);
        }
        //if not, should we restart?
        else if(loop) {
            //go to index 0 and get timer length again
            currWaveIndex = 0;
            secs_till_next_enemyWave = enemyWaves.SecsTillSpawn(currWaveIndex);

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
        }
        
    }


    public GameObject GetEnemyAtIndex(int index){
        return enemies[index];
    }

}
