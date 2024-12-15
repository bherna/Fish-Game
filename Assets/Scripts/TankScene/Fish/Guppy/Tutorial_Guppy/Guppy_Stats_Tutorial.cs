using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Stats_Tutorial : Guppy_Stats
{

    // we just override hungry and ate functoins to send that info to tutorial
    protected override void Start() {
        
        base.Start();

        //if the tutorial is still before index 2 (learning to feed guppy)
        if(Controller_Tutorial.instance.index < 2){
            //set burnRate to 0
            //this makes it so guppy can't get hungry until player triggers guppy to be allowd to get hungry section
            burnRate = 0;
        }

        //now we do the same for ageing
        //check to see if we are before the 6th index (learning about collecting coins)
        if(Controller_Tutorial.instance.index < 6){
            updateAge = false;
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

    public void GuppyCanAgeNow(){
        updateAge = true;
    }



    public new void Died(bool playSound = true){

        //if the guppy dies then tell tutorial that guppy died
        //else tutorial gets stuck and player doesn't know da f went on
        Controller_Tutorial.instance.GuppyStarved();

        base.Died();
    }


}
