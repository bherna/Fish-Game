

//each level is going to have its own tank enemywave obj
//which hold information on how this level will spawn its enemies
[System.Serializable]
public class Tank_EnemyWaves 
{

    //wave matrix to hold every wave with respective enemies
    //main list -> wave list
    //wave list -> enemy prefab list
    public Single_EnemyWave[] waves;

    public bool loop;

    public Tank_EnemyWaves(Single_EnemyWave[] waves, bool loop){
        this.waves = waves;
        this.loop = loop;
    } 
    
    //how many enemy waves does this tank have
    public int TotalWavesCount(){
        return waves.Length;
    }
    //get Single EnemyWave
    public string[] GetWave(int waveIndex){
        return waves[waveIndex].enemies;
    }
    //get a enemy wave's total seconds before we spawn
    public int SecsTillSpawn(int waveIndex){
        return waves[waveIndex].secTillWaveSpawn;
    }

}



//this is a single enemy wave for a tank
//each tank will have a list of single_enemywaves objs to make up one tank_enemywave obj
[System.Serializable]
public struct Single_EnemyWave
{
    //how much time until this wave will spawn
    public int secTillWaveSpawn;

    //list of enemies (names) to spawn (can also be what ever else dev wants)
    public string[] enemies; 

}



