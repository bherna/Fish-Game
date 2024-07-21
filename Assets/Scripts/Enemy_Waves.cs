using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct EnemyWave
{
    [SerializeField] public int secTillWaveSpawn;
    [SerializeField] public List<GameObject> wave; //list of enemies to spawn (can also be what ever else dev wants)
}


public class Enemy_Waves : MonoBehaviour
{

    //wave matrix to hold every wave with respective enemies
    //main list -> wave list
    //wave list -> enemy prefab list
    [SerializeField] public EnemyWave[] wave_mat; 
    
    public int GetLength(){
        return wave_mat.Length;
    }
    public List<GameObject> Index_GetWave(int waveIndex){
        return wave_mat[waveIndex].wave;
    }
    public int Index_GetTimeTillSpawn(int waveIndex){
        return wave_mat[waveIndex].secTillWaveSpawn;
    }


    
}
