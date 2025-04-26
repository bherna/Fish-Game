using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetReq_Charlie : PetReq_ParentClass
{

    //fyi charlie is the level 1-1 pet, tutorial level
    
    //only goal for charlie to hatch is 4 adult guppys in the tank
    private const int adults_req = 4;
    private int adults = 0;

    //no income for charlile to worry about





    public override void StartReqs()
    {
        base.StartReqs();

        //when we start, we need to get all adult guppys
        adults = Controller_Fish.instance.HowManyGuppysAtAge_(2);

        //update requiremtns board + checker (last)
        PostUpdates();

    }



    //everytime a new update to our reqs changes, we update the visuals, and we chceck to see if we completed reqs
    protected override void PostUpdates(){

        if(!toggle){return;}

        string ourTex = string.Format("Requirements:\n~~~~~~~~~~~~~~~~\nAdult Guppys: {0} / {1}", adults, adults_req);
        Controller_Requirements.instance.UpdateReqs(ourTex);


        //did we complete our reqs
        if(adults >= adults_req){
            
            //we done, and we can stop
            toggle = false;

            //run post game thingy
            Controller_Objective.instance.LevelComplete();

        }

    }



    //similar to the mary baby checker counter, 
    //this is just for adults
    //only way to become an adult is to get birthday'd
    //onlly way to leave is through died event, or birthday into gemfish stage
    public override void UpdateGuppyCounter_Age(int age, int val){

        //if age is adult, we do something
        switch(age){

            case 2:
                //since this is just adult age we increment by val
                adults += val;
                break;

            case 3:
                //not sure if we want to do something about gemfish right now
                //as of this update, gemfish will just count as a type of adult fish, so just do nothing here
                break;
        }

        
        //update requiremtns board + checker
        PostUpdates();

                
    }
}
