using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller_Main : MonoBehaviour
{

    //fish contoller
    [SerializeField] GameObject food_c;

    //food controller
    [SerializeField] GameObject fish_c;

    //Fish tank
    [SerializeField] GameObject tank_c;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(tank_c, new Vector2(0,0), Quaternion.identity);
        Instantiate(food_c, new Vector2(0,0), Quaternion.identity);

        //fish last, to get food_c reference
        Instantiate(fish_c, new Vector2(0,0), Quaternion.identity);
       

    }

    
}
