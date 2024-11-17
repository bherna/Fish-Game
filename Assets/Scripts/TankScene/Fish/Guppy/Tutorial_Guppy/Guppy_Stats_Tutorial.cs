using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Stats_Tutorial : Guppy_Stats
{

    // we just override hungry and ate functoins to send that info to tutorial


    protected override void GuppyHungry(){

        base.GuppyHungry();

        //send message to tutorial
        Controller_Tutorial.instance.GuppyHungry();
    }



    public override void GuppyEated(int foodValue){

        base.GuppyEated(foodValue);
        
        //send message to tutorial
        Controller_Tutorial.instance.GuppyAte();
    }

}
