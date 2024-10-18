using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] int rectTransform_width = 1920;
    [SerializeField] EventOnHover_PlayButton playButton;
    [SerializeField] GameObject temp;
    [SerializeField] RectTransform panel;


    private int curr_screen = 0;

    private string currSceneSet;



    private void Start() {

        //set ui tabs positions
        for (int i = 0; i <= transform.childCount-1; i++){
            transform.GetChild(i).gameObject.transform.localPosition = new Vector3(rectTransform_width*i,0,0); //set their pos
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    

    //set scene currSceneSet
    //this lets us move from level select to pet select
    public void GoToPets(string sceneName){
        currSceneSet = sceneName;

        //go to pets (should be last scene)
        GoToPetsPanel();
    }

    //Go to scene currSceneSet
    public void PlayLevel(){

        SceneManager.LoadScene(currSceneSet);
    }



    public void QuitApp(){
        Application.Quit();
        Debug.Log("application has quit.");
    }

    public void Next_UIScreen(){

        for (int i = 0; i <= transform.childCount-1; i++){
            transform.GetChild(i).gameObject.transform.localPosition -= new Vector3(rectTransform_width,0,0);
        }

        curr_screen += 1;
    }

    public void Previous_UIScreen(){

        for (int i = 0; i <= transform.childCount-1; i++){
            transform.GetChild(i).gameObject.transform.localPosition += new Vector3(rectTransform_width,0,0);
        }

        curr_screen -= 1;

        //logic to reset the play button to turn of the lights in the fish tank
        if(curr_screen == 0){
            playButton.OnPointerReturnToTitleScreen();
        }
    }

    //last scene is the pet panel
    private void GoToPetsPanel(){

        //number of transistions until last scene
        int lastScene = transform.childCount - (curr_screen+1);

        for (int i = 0; i <= transform.childCount-1; i++){
            transform.GetChild(i).gameObject.transform.localPosition -= new Vector3(rectTransform_width * lastScene,0,0);
        }


        //make pets move to there correct spots
        var screenPos = panel.transform.position;
        screenPos.z = Vector3.Dot(Camera.main.transform.forward, - Camera.main.transform.position);
        temp.transform.position  = Camera.main.ScreenToWorldPoint(screenPos); 
        

    }

    //if player want to leave pet selection -> return to level selection, return to world panel they were last on
    public void ReturnToLevelSelection(){

        for (int i = 0; i <= transform.childCount-1; i++){
            transform.GetChild(i).gameObject.transform.localPosition = new Vector3(rectTransform_width * curr_screen,0,0);
        }
    }
}
