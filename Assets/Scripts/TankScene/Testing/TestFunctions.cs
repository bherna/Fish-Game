
using UnityEngine;


//first class holds all methods that the tester can use
//we only use this class as a reference, dont use start or update methods here

public class TestFunctions : MonoBehaviour 
{
    public int timeSpeed = 1;

    public void UpdateTimeSpeed(){
        Time.timeScale = timeSpeed;
    }
}

