using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Controller_Timer : MonoBehaviour
{
    //our ui timer text 
    [SerializeField] TextMeshProUGUI timer_text;


    //timer
    private float timerCount = 0;

    //used for pausing the timer
    private bool dontCount = false;



    //singleton this class
    public static Controller_Timer instance {get; private set; }
    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }  


    // Start is called before the first frame update
    void Start()
    {
        timer_text.text = string.Format("00:00");

    }

    //so far only used in stopping timer when level is complete
    public void StopTimer(){
        dontCount = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(dontCount){return;}
        
        //increment
        timerCount += Time.deltaTime;
        UpdateTime();
    }

    private void UpdateTime(){

        int min = (int)math.floor(timerCount/60);
        int sec = (int)timerCount % 60;
        
        //show
        timer_text.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public float GetFinalTime(){

        return timerCount;
        
    }

}
