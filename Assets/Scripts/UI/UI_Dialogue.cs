using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



[System.Serializable]
public struct Dialogue_Expect
{
    [SerializeField] public string line;  //dialogue string
    [SerializeField] public Expect_Type expect_Type; //what dialogue string expects, to run
    
}


public class UI_Dialogue : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] Dialogue_Expect[] dialogue_Expect;
    public Expect_Type curr_expectType {get; private set; } 

    private int index;    
    private float textSpeed = 0.05f;
    

    //start method
    public void StartDialogue(){

        //set string to empty/ index to 0
        textUI.text = string.Empty;
        index = 0;

        //set our next index expect type
        curr_expectType = dialogue_Expect[index+1].expect_Type;

        //start dialogue
        StartCoroutine(TypeLine());
    }
    

    //This gets used outside of this class
    //When ever a 'click' is done this command is played
    //click helps move dialogue to next line / finish current dialogue line
    //returns nextline bool (true for more lines, false for we are done)
    //else return true , since we have another click togo
    public bool Click(){

        if(textUI.text == dialogue_Expect[index].line){
            return NextLine();
        }
        else{
            StopAllCoroutines();
            textUI.text = dialogue_Expect[index].line;
            return true;
        }
        
    }

    
    //slowly types out the current dialogue string we are index'd at
    private IEnumerator TypeLine(){
        //for each char 1 by 1
        foreach( char c in dialogue_Expect[index].line.ToCharArray()){

            textUI.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }


    //used for checking if a next line of dialogue/string exists, then starts that coroutine
    //if next line exists, return true
    //else false
    private bool NextLine(){
        if (index < dialogue_Expect.Length - 1){

            //increment index / reset ui text box / set next expect type
            index++;
            textUI.text = string.Empty;
            curr_expectType = dialogue_Expect[index+1].expect_Type;
            //start typing line method and return
            StartCoroutine(TypeLine());
            return true;
        }
        else{

            return false;
        }
    }


    
}