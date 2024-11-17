using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Money : MonoBehaviour
{
    //how often a fish drops money (in seconds)
    protected float secTillMoney = 4f;

    //current time (in delta time)
    protected float currTime;

    [SerializeField] Guppy_Stats guppy_Stats;

    //money prefabs
    [SerializeField] GameObject coin_silver; //teen
    [SerializeField] GameObject coin_gold; //adult

    private void Start() {
        guppy_Stats = GetComponent<Guppy_Stats>();
    }
    

    // Update is called once per frame
    void Update()
    {

        //drop money
        switch(guppy_Stats.current_age_stage){

            case 0:
                //do nothing
                break;
            case 1:
                //drop money
                currTime += Time.deltaTime;//update timer
                DropMoney(coin_silver);
                break;
            case 2:
                //drop money              
                currTime += Time.deltaTime;//update timer
                DropMoney(coin_gold);
                break;
            default:
                Debug.Log("Should not be this old...");
                break;
        }
        
    }

   

    protected virtual void DropMoney(GameObject coinType){

        
        if(currTime >= secTillMoney){

            //reset timer
            currTime = 0;

            //drop coin
            //behind fish
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z+2);
            Instantiate(coinType, pos, Quaternion.identity);
        }
    }
}
