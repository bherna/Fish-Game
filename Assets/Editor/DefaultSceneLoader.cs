#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoadAttribute]
public static class DefaultSceneLoader
{

    /*
    https://stackoverflow.com/questions/35586103/unity3d-load-a-specific-scene-on-play-mode
    
    3Dynamite:
    
        I made this simple script that loads the scene at index 0 in the build settings when you push Play. I hope someone find it useful.

        It detects when the play button is push and load the scene. Then, everything comes back to normal.

        Oh! And it automatically executes itself after opening Unity and after compile scripts, so never mind about execute it. Simply put it on a Editor folder and it works.
    */

    static DefaultSceneLoader(){
        EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    static void LoadDefaultScene(PlayModeStateChange state){
        if (state == PlayModeStateChange.ExitingEditMode) {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ();
        }

        if (state == PlayModeStateChange.EnteredPlayMode) {
            EditorSceneManager.LoadScene (0);
        }
    }
}
#endif
