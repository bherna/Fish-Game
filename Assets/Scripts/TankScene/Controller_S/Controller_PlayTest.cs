using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// This class is made to make it easier to test features within a level
//       Make sure to remove this script/ gameobject from game

public class Controller_PlayTest : MonoBehaviour
{


    public int TimeSpeed = 1;


    // Update is called once per frame
    void Update()
    {

        //update how fast our time
        if(Input.GetKeyUp(KeyCode.Backslash)){

            //
            Time.timeScale = TimeSpeed;
        }
    }
}
