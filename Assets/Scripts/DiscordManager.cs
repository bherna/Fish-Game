using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DiscordManager : MonoBehaviour
{
    //for the different activity states we will be in
    public enum DiscordState { Menu, Level };

    private Discord.Discord discord; //discord instance
  

    public static DiscordManager instance { get; private set; }
    void Awake()
    {

        //delete duplicate of this instance

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            discord = new Discord.Discord(1434678442762436684, (ulong)Discord.CreateFlags.NoRequireDiscord);
            DontDestroyOnLoad(gameObject);
        }
    }



    public void OnDisable()
    {
        if(discord != null)
        {
            discord.Dispose();
        }
        Debug.Log("discord is disposed");
    }



    //discord state = where the player is currenly at
    // world and level is when they are playing a level
    public void ChangeActivity(DiscordState discordState, int world, int level)
    {
        //will/need  this
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity { }; //have to set like this so i can use it to  be set at the bottom

        switch (discordState)
        {
            case DiscordState.Menu:

                activity = new Discord.Activity
                {
                    State = "In Main Menu",
                    Details = "is Chilling With Fishes",
                    Assets = { LargeImage = "titlecard_words", LargeText = "Project - Fish Shop" },
                };
                break;

            case DiscordState.Level:
                activity = new Discord.Activity
                {
                    State = String.Format("Current Level ({0}-{1})", world, level),
                    Details = "is Playing Story Level",
                    Assets = { LargeImage = "titlecard_words", LargeText = "Project - Fish Shop" },
                };
                break;

            default:
                //should not be in here
                Debug.Log("This should not be possible");
                break;
        }

        //and finaly set
        activityManager.UpdateActivity(activity, (res) => { Debug.Log("Activity Updated !"); });

    }



    void Update()
    {
        discord.RunCallbacks();
    }


}
