using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet_MaryFertile_Collision : FishCollision_ParentClass
{

    private Pet_MaryFertile maryScript;


    void Start()
    {
        maryScript = GetComponent<Pet_MaryFertile>();
    }


    private void OnTriggerStay2D(Collider2D other)
    {

        if (Pet_States.depressed == maryScript.curr_PetState && other.gameObject.CompareTag("Food"))
        {
            //eat + destroy obj
            var foodscript = other.GetComponent<Drop_Food>();
            switch (foodscript.foodType)
            {

                case FoodTypes.feed:
                case FoodTypes.burger:
                    Controller_Food.instance.TrashThisFood(other.gameObject);
                    maryScript.EatedFood();
                    break;

                default:
                    Debug.Log("No food type selected");
                    break;
            }
        }
    }

    
}
