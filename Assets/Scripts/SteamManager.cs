using System;
using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

public class SteamManager : MonoBehaviour
{

    private void Awake(){
        try
        {
            SteamClient.Init( 3099090 );
        }
        catch (Exception)
        {
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
            Debug.Log("Count not initialize steam client.");
        }

        //ensure steam manager cannot be destroyed in changing scenes
        DontDestroyOnLoad(this.gameObject);


        test();
    }

    private void test() {

        
        var playername = SteamClient.Name;
        var playersteamid = SteamClient.SteamId;

        Debug.Log("player name: "+playername.ToString());
        Debug.Log("steam id: "+playersteamid.ToString());

        /*
        int value = 0;
        SteamScreenshots.TriggerScreenshot();

        Steamworks.SteamUserStats.SetStat( "deaths", value );

        foreach ( var item in Steamworks.SteamInventory.Items )
        {
            Debug.Log( $"{item.Def.Name} x {item.Quantity}" );
        }

        foreach ( var player in SteamFriends.GetFriends() )
        {
            Debug.Log( $"{player.Name}" );
        }
        */
    }


    private void OnDisable() {
        
        //won't actually shutdown the editor when leaving playmode
        SteamClient.Shutdown();
    }

    private void Update(){

        //keep connection callbacks with steam
        //This allows Steam to think and run any callbacks that are waiting.
        SteamClient.RunCallbacks();
    }
}
