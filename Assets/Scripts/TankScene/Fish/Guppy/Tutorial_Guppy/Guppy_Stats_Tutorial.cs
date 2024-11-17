using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Stats_Tutorial : Guppy_Stats
{

    // we just override hungry and ate functoins to send that info to tutorial
    protected override void Start() {
        
        base.Start();

        //if the tutorial is still before index 3 (learning to feed guppy)
        if(Controller_Tutorial.instance.index < 3){
            //set burnRate to 0
            //this makes it so guppy can't get hungry until player triggers guppy to be allowd to get hungry section
            burnRate = 0;
        }
        
    }

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


    public void GuppyCanEatNow(){
        burnRate = 1;
    }

}
