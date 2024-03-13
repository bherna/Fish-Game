using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct EnemyWave
{
    [SerializeField] public int secTillWaveSpawn;
    [SerializeField] public List<GameObject> wave;
}


public class Enemy_Waves : MonoBehaviour
{

    //wave matrix to hold every wave with respective enemies
    //main list -> wave list
    //wave list -> enemy prefab list
    [SerializeField] public EnemyWave[] wave_mat; 

    //enemy prefabs
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;

    

    public EnemyWave[] Get_WaveMat(){

        return wave_mat;
    }
    
}
