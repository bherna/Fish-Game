using UnityEngine;

public class HyperLink : MonoBehaviour
{
    public void OpenYouTube(){
        OpenURL("https://www.youtube.com/@MeowFruits");
    }

    public void OpenDiscord(){
        OpenURL("https://discord.gg/x8TcTQcFNF");
    }

    public void OpenBluesky(){
        OpenURL("https://bsky.app/profile/meowfruit.bsky.social");
    }

    public void OpenURL(string link){
        Application.OpenURL(link);
    }
}
