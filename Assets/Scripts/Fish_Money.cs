using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish_Money : MonoBehaviour
{

    //how often a fish drops money (in seconds)
    [SerializeField] float secTillMoney = 2f;

    //current time (in delta time)
    private float currTime;

    //money prefabs
    [SerializeField] GameObject coin;
    [SerializeField] Fish_Age fish_Age;

    [SerializeField] int teenCoinVal = 10;
    [SerializeField] int adultCoinVal = 15;

    

    // Update is called once per frame
    void Update()
    {

        //drop money
        switch(fish_Age.current_age_stage){

            case 0:
                //do nothing
                break;
            case 1:
                //drop money
                currTime += Time.deltaTime;//update timer
                DropMoney(teenCoinVal);
                break;
            case 2:
                //drop money              
                currTime += Time.deltaTime;//update timer
                DropMoney(adultCoinVal);
                break;
            default:
                Debug.Log("Should not be this old...");
                break;
        }
        
    }

   

    private void DropMoney(int moneyVal){

        
        if(currTime >= secTillMoney){

            //reset timer
            currTime = 0;

            //drop coin
            var temp = Instantiate(coin, transform.position, Quaternion.identity);
            temp.GetComponent<EventClick_Coin>().UpdateCoinVal(moneyVal);
        }
    }



}
