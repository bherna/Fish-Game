using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetReq_Salt : PetReq_ParentClass
{
    
    //salt's egg whats us to feed guppys 52 times and have max food from shop
    private int feeds = 0;
    private const int feeds_req = 52;

    private bool maxFood = false;
    private string foodText = "No";


    public override void StartReqs()
    {
        base.StartReqs();

        //update parent variables
        income_req = 160;

        //update requiremtns board + checker (last)
        PostUpdates();

        //start the income caller, since we want to keep track of that stuff
        StartCoroutine(Controller_Wallet.instance.CalculateIncome());

    }


    //everytime a new update to our reqs changes, we update the visuals, and we chceck to see if we completed reqs
    protected override void PostUpdates(){

        if(!toggle){return;}

        string ourTex = string.Format(
            "Requirements:\n~~~~~~~~~~~~~~~~\nFeed Guppys: {0} / {1}\n\nBought Max Food Power: {2}\n\nIncome: ${3} / ${4}",
            feeds, feeds_req, foodText, income, income_req);
        Controller_Requirements.instance.UpdateReqs(ourTex);


        //did we complete our reqs
        if(feeds >= feeds_req && income >= income_req && maxFood){
            
            //we done, and we can stop
            toggle = false;

            //run post game thingy
            Controller_Objective.instance.LevelComplete();

        }

    }



    //run everytime a gupppy succcessfully eats a pellet
    public override void GuppyAte(){

        //only run if reqs are turned on
        if(toggle){

            feeds++;

            //now update+check
            PostUpdates();
        }
    }



    public override void MaxFoodReached(){
        maxFood = true;
        foodText = "Yes";
    }

}
