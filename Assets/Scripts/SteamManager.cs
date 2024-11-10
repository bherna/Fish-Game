using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamManager : MonoBehaviour
{
    private void Awake(){
        try
        {
            Steamworks.SteamClient.Init( 252490 );
        }
        catch ( System.Exception e )
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
    }


    private void OnDisable() {
        
        //won't actually shutdown the editor when leaving playmode
        Steamworks.SteamClient.Shutdown();
    }

    private void Update(){

        //keep connection callbacks with steam
        //This allows Steam to think and run any callbacks that are waiting.
        Steamworks.SteamClient.RunCallbacks();
    }
}
