using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] List<GameObject> Panels;
    [SerializeField] int rectTransform_width = 1920;
    [SerializeField] EventOnHover_PlayButton playButton;

    private int curr_screen = 0;

    private void Start() {

        for (int i = 0; i <= Panels.Count-1; i++){
            Panels[i].transform.localPosition = new Vector3(rectTransform_width*i,0,0); //set their pos
            Panels[i].SetActive(true);
        }
        

    }
    public void GoToScene(string sceneName){

        SceneManager.LoadScene(sceneName);
    }

    public void QuitApp(){
        Application.Quit();
        Debug.Log("application has quit.");
    }

    public void Next_UIScreen(){

        foreach(GameObject panel in Panels){

            panel.transform.localPosition -= new Vector3(rectTransform_width,0,0);
        }

        curr_screen += 1;
    }

    public void Previous_UIScreen(){

        foreach(GameObject panel in Panels){

            panel.transform.localPosition += new Vector3(rectTransform_width,0,0);
        }

        curr_screen -= 1;

        if(curr_screen == 0){
            playButton.OnPointerReturnToTitleScreen();
        }
    }


    
}
