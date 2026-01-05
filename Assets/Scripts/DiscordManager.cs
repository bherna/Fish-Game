using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class DiscordManager : MonoBehaviour
{
    /*
    //for the different activity states we will be in
    //Menu: we are in the main menu of the game
    //Level: we are in a level
    public enum DiscordState { Menu, Level };

    private Discord.Discord discord; //discord instance
  
    //if our discord app was not booted
    private bool discordClosed = false;
    private bool isFirst = false;

    public static DiscordManager instance { get; private set; }
    private void OnEnable() 
    {

        //delete duplicate of this instance
        //only difference with this singleton is that we need to keep 
        //the first one alive always to make discord app work

        //if instance is initally empty, then this one is the first one to be make
        if (instance == null)
        {
            isFirst = true;
            instance = this;
            BootUpDiscord();
            DontDestroyOnLoad(gameObject);
        }
        //if we are not empty, so this is the N+1 moment/ not the first pass through
        //if we are not the first then delete
        else if(!isFirst)
        {
            UnityEngine.Debug.Log(string.Format("isfirst: {0}",isFirst));
            UnityEngine.Debug.Log("This discord game object was deleted");
            Destroy(gameObject);
        }
    }



    public void OnDestroy()
    {
        if(discord != null)
        {
            discord.Dispose();
        }
        UnityEngine.Debug.Log("discord is disposed");
    }



    private void BootUpDiscord()
    {
        try
        {
            discord = new Discord.Discord(1434678442762436684, (ulong)Discord.CreateFlags.NoRequireDiscord);
            discordClosed = false;
        }
        catch (ResultException e)
        {
            UnityEngine.Debug.Log(e);
            discordClosed = true;
        }
    }


    //discord state = where the player is currenly at
    // world and level is when they are playing a level
    public void ChangeActivity(DiscordState discordState, int world, int level)
    {
        if(discordClosed){return;}
        UnityEngine.Debug.Log("If i dont have this debug here, this function doesn't update the activity for some reason. Some Schrodinger type-sh");

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
                UnityEngine.Debug.Log("This should not be possible");
                break;
        }

        //and finaly set
        activityManager.UpdateActivity(activity, (res) => { UnityEngine.Debug.Log("Activity Updated !"); });

    }



    void Update()
    {
        if(discordClosed){return;}

        try
        {
            discord.RunCallbacks();
        }
        catch(ResultException e)
        {
            UnityEngine.Debug.Log(e);
            discordClosed = true;
        }
    }


*/

}
