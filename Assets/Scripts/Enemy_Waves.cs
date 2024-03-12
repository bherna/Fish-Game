using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Waves : MonoBehaviour
{

    //wave matrix to hold every wave with respective enemies
    //main list -> wave list
    //wave list -> enemy prefab list
    [SerializeField] List<List<GameObject>> wave_mat; 

    //enemy prefabs
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;

    private void Start() {

        //create enemy waves
        wave_mat = new List<List<GameObject>>
        {
            new List<GameObject>(), 
            new List<GameObject>(), 
            new List<GameObject>(),
            new List<GameObject>()
        };

        wave_mat[0].Add(enemy3);
        wave_mat[0].Add(enemy1);

        wave_mat[1].Add(enemy2);

        wave_mat[2].Add(enemy3);
    }

    public List<List<GameObject>> Get_WaveMat(){

        return wave_mat;
    }
    
}
