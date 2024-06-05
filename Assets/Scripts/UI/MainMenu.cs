using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] List<Transform> Panels;
    [SerializeField] int rectTransform_width = 1920;

    private void Start() {

        for (int i = 0; i <= Panels.Count-1; i++){
            Panels[i].transform.localPosition = new Vector3(rectTransform_width*i,0,0);
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

        foreach(Transform panel in Panels){

            panel.transform.localPosition -= new Vector3(rectTransform_width,0,0);
        }
    }

    public void Previous_UIScreen(){

        foreach(Transform panel in Panels){

            panel.transform.localPosition += new Vector3(rectTransform_width,0,0);
        }
    }
}
