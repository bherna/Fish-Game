using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.IO;



[System.Serializable]
public class WholeDialogue{
    public string[] script; //same as json file
}


public class UI_Dialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textUI;

    private string[] script;

    private int index = 0;   //Keeps track of current string[] we are displaying
    private float textSpeed = 0.05f;


    

    //start method
    //this is used in the controller_tutorial script
    //return true if we have a tutorial to run
    //else return false to skip running tutorial 
    public bool StartDialogue(){

        //reset values
        //set string to empty/ index to 0
        textUI.text = string.Empty;

        return GetJsonScriptNumber("1");
        
    }


    //Used in opening the next json file that holds our next script for tutorial
    //if the json file exsists, then we have a script to run, return true
    //else return false
    //we also update our script in here, so we don't have to return it

    //For each tutorial, there should be a list of json files
    //The naming convention should be 'Tank_{0}-{1}'
    //{0} == the tank world we are using, 
    //{1} == this is the script index, we start at 1 and increment from there
    public bool GetJsonScriptNumber(string scriptNum){

        //Import in the json file that this tank_world-leve will use
        string filePath = "Tutorial/Tank_" + GameVariables.GetTankWorld()+"-"+scriptNum;

        //get the json file we want to read
        string targetFile = Resources.Load<TextAsset>(filePath).text;
        script = JsonUtility.FromJson<WholeDialogue>(targetFile).script;

        //does this file (tank_world-level.json) exsist
        if(targetFile != null){
            //if yes, then we have a tutorial to run
            //start dialogue
            StartCoroutine(TypeLine());
            return true;
        }
        else{
            return false;
        }
    }
    

    //This gets used in the controller_tutorial class
    //When ever a 'click' is done this command is played
    //click helps move dialogue to next line / finish current dialogue line
    //returns nextline bool (true for more lines, false for we are done)
    //else return true , since we have another click togo
    public bool Click(){

        //when our entire dialogue is in the text box, we can move on to next dialogue
        if(textUI.text == script[index]){
            return NextLine();
        }
        //our dialogue still hasn't finished printing, so insta finish it
        else{
            StopAllCoroutines();
            textUI.text = script[index];
            return true;
        }
        
    }

    
    //slowly types out the current dialogue string we are index'd at
    private IEnumerator TypeLine(){
        //for each char 1 by 1
        foreach( char c in script[index].ToCharArray()){

            textUI.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }


    //used for checking if a next line of dialogue/string exists, then starts that coroutine
    //if next line exists, return true
    //else false
    private bool NextLine(){
        //do we have next line
        if (index+1 < script.Length){
            
            //increment index / reset ui text box / set next expect type
            index++;
            textUI.text = string.Empty;

            //start typing line method and return
            StartCoroutine(TypeLine());
            return true;
        }
        else{
            //no more lines
            //reset values now
            index = 0;
            textUI.text = string.Empty;
            return false;
        }
    }


    //used for disabling or enabling in showing text ui box
    public void ToggleDialogueBox(bool toggle){

        transform.GetChild(0).gameObject.SetActive(toggle);
        transform.GetChild(1).gameObject.SetActive(toggle);
    }


}
