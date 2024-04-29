using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{


    //used for counting to 1
    public float TimerCount;

    //used for pausing the timer
    public bool TimerOn = false;

    //current seconds count
    public int currentTime = 0;

    //our ui timer text 
    [SerializeField] TextMeshProUGUI timer_text;

    // Start is called before the first frame update
    void Start()
    {
        TimerOn = true;
        UpdateTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if(TimerOn){

            //count to one
            if(TimerCount >= 1){
                TimerCount = 0;
                currentTime += 1;
                UpdateTimerText();
            }
            else{
                TimerCount += Time.deltaTime;
            }
        }
    }


    //updates the ammount of minutes and seconds for timer text
    void UpdateTimerText(){
        
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timer_text.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

}
