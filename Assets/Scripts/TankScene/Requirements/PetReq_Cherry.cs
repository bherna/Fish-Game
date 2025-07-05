using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetReq_Cherry : PetReq_ParentClass
{

    //cherry's egg wants food pellets to dissolve onto them
    private int foodDissolved;
    private const int foodDissolved_req = 30; //idk how many



    public override void StartReqs()
    {
        base.StartReqs();

        //update parent variables
        income_req = 200;

        //update requiremtns board + checker (last)
        PostUpdates();

    }


    protected override void PostUpdates()
    {

        if (!toggle) { return; }

        string ourTex = string.Format(
            "Requirements:\n~~~~~~~~~~~~~~~~\nFood Dissolved on Egg: {0} / {1}\n\nIncome: ${2} / ${3}",
            foodDissolved, foodDissolved_req, income_cur, income_req);
        Controller_Requirements.instance.UpdateReqs(ourTex);


        //did we complete our reqs
        if (foodDissolved >= foodDissolved_req && income_cur >= income_req)
        {

            //we done, and we can stop
            toggle = false;

            //run post game thingy
            Controller_Objective.instance.LevelComplete();

        }

    }


    //update # of food disolved
    public override void FoodDissolved()
    {
        foodDissolved += 1;
        PostUpdates();
    }
}
