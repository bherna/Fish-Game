using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Money_Tutorial : Guppy_Money
{
    //functions hers are overriden for messaging controller tutorial



    protected override void DropMoney(GameObject coinType){

        
        if(currTime >= secTillMoney){

            //reset timer
            currTime = 0;

            //drop coin
            //behind fish
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z+2);
            Instantiate(coinType, pos, Quaternion.identity);

            //message tutorial
            Controller_Tutorial.instance.GuppyDropCoin();
        }
    }
}
