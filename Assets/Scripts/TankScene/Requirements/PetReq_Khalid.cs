using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetReq_Khalid : PetReq_ParentClass
{
    

    //khalid's egg whats adult guppys to be sacrificed during en enemy wave
    private int ADeaths = 0;
    private const int ADeaths_req = 6;


     public override void StartReqs()
    {
        base.StartReqs();

        //update parent variables
        income_req = 150;

        //update requiremtns board + checker (last)
        PostUpdates();

    }


    //everytime a new update to our reqs changes, we update the visuals, and we chceck to see if we completed reqs
    protected override void PostUpdates(){

        if(!toggle){return;}

        string ourTex = string.Format("Requirements:\n~~~~~~~~~~~~~~~~\nAdult Deaths during Enemy Wave: {0} / {1}\n\nIncome: ${2} / ${3}", ADeaths, ADeaths_req, income_cur, income_req);
        Controller_Requirements.instance.UpdateReqs(ourTex);


        //did we complete our reqs
        if(ADeaths >= ADeaths_req && income_cur >= income_req){
            
            //we done, and we can stop
            toggle = false;

            //run post game thingy
            Controller_Objective.instance.LevelComplete();

        }

    }



    //we only care if adults (or anything above an adult) dies during an enemy wave so
    public override void UpdateGuppyCounter_Age(int age, int val){

        if(age >= 2 && val == -1 && Controller_Enemy.instance.currently_in_wave){

            ADeaths++;
        }

        
        //update requiremtns board + checker
        PostUpdates();

                
    }


}
