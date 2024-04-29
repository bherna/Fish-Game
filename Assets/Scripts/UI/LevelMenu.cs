using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    
    public void GoToLevel(string levelName){
        SceneManager.LoadScene(levelName);
    }
}
