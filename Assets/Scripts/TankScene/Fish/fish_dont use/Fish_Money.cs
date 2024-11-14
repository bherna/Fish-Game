using UnityEngine;

public class Fish_Money : MonoBehaviour
{

    //how often a fish drops money (in seconds)
    [SerializeField] float secTillMoney = 2f;

    //current time (in delta time)
    private float currTime;

    
    [SerializeField] Fish_Age fish_Age;

//money prefabs
    [SerializeField] GameObject coin_silver; //teen
    [SerializeField] GameObject coin_gold; //adult

    

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

   

    private void DropMoney(GameObject coinType){

        
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
