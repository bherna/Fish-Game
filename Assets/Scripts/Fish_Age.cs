using System;
using System.Collections.Generic;
using UnityEngine;


public class Fish_Age : MonoBehaviour
{

    //reference to fish dropping money script
    [SerializeField] Fish_Money fish_money;

    //dictionary to hold fish stage, and coin value
    private Dictionary<string, int> fish_stages = new Dictionary<string, int>();

    private int current_age_stage = 0;

    //fish current age until next fish_stage
    [SerializeField] float fish_curr_age = 0;

    //takes how many seconds till next fish stage
    [SerializeField] int until_next_stage = 100;

    //bool if we should keep age-ing
    private bool updateAge = true;


    // Start is called before the first frame update
    void Start()
    {
        fish_stages.Add("Baby", 0);
        fish_stages.Add("Teen", 10);
        fish_stages.Add("Adult", 20);
    }

    // Update is called once per frame
    void Update()
    {

        if(updateAge){

            //update age of fish
            fish_curr_age += Time.deltaTime;

            if(fish_curr_age > until_next_stage){
                UpdateFishStage();
            }
        }
        
           
    }

    private void UpdateFishStage(){

        //if current age is not final stage
        if(current_age_stage < fish_stages.Count){
            
            //update 
            current_age_stage += 1;

            //reset
            fish_curr_age = 0;
        }
        else{
            //we are done aging
            updateAge = false;
        }
    }
}
