using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Collision : MonoBehaviour
{
    // --------------------------------- gubby script reference --------------------------------- 
    private Guppy_Stats guppy_Stats;
    private Guppy_SM guppy_SM;

    private void Start() {
        guppy_Stats = GetComponent<Guppy_Stats>();
        guppy_SM = GetComponent<Guppy_SM>();
    }

    private void OnTriggerStay2D(Collider2D other){

        //              FOOD
        //if fish is hungry and we collided with food
        if(Guppy_States.hungry == guppy_SM.guppy_current_state && other.gameObject.CompareTag("Food"))
        {

            //eat + destroy obj
            var foodValue = other.GetComponent<FoodValue>().GetFoodValue();
            Controller_Food.instance.TrashThisFood(other.gameObject);

            guppy_Stats.FishEated(foodValue);
        }
    }
}
