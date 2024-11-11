using System.Collections.Generic;
using UnityEngine;



//each level is going to have its own tank enemywave obj
//which hold information on how this level will spawn its enemies
public class Tank_EnemyWaves 
{

    //wave matrix to hold every wave with respective enemies
    //main list -> wave list
    //wave list -> enemy prefab list
    public List<Single_EnemyWave> waves; 

    public Tank_EnemyWaves(Tank_EnemyWaves tank_EnemyWaves){
        this.waves = tank_EnemyWaves.waves;
    }
    
    //how many enemy waves does this tank have
    public int TotalWavesCount(){
        return waves.Count;
    }
    //get Single EnemyWave
    public List<string> GetWave(int waveIndex){
        return waves[waveIndex].enemies;
    }
    //get a enemy wave's total seconds before we spawn
    public int SecsTillSpawn(int waveIndex){
        return waves[waveIndex].secTillWaveSpawn;
    }

}



//this is a single enemy wave for a tank
//each tank will have a list of single_enemywaves objs to make up one tank_enemywave obj
public struct Single_EnemyWave
{
    //how much time until this wave will spawn
    public int secTillWaveSpawn;

    //list of enemies (names) to spawn (can also be what ever else dev wants)
    public List<string> enemies; 

}



