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

    //
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //update timer
        currTime += Time.deltaTime;

        if(currTime >= secTillMoney){

            //reset timer
            currTime = 0;

            //drop coin
            Instantiate(coin, transform.position, Quaternion.identity);

        }
    }

    public void updateMoneyVal(){
        
    }



}
