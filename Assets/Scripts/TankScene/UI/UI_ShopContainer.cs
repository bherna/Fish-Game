using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//all this script should do is add a listener to each of the child objects
// - if the child obj has a button component then add listener
// - else skip, but still increment index

//it also enables and disables buttons interactive
//for tutorial use
public class UI_ShopContainer : MonoBehaviour
{
    //list to hold each index of each button, else we end up using the same button_index
    private List<int> activeButtonIndex;
    
    private void OnEnable() {
        
        activeButtonIndex = new List<int>();
        
        for(int index = 0; index < transform.childCount; index++){
            try{
                //add to list 
                int newI = index;
                activeButtonIndex.Add(newI);
                //check for button component -> add listener
                transform.GetChild(index).GetComponent<Button>().onClick.AddListener(delegate{ListenForClick(newI);});
                
            }
            catch(Exception){
                
                //remove new list addition
                activeButtonIndex.Remove(activeButtonIndex.Count);
            }
            
        }
    }



    //used in tutorial for acting as a click
    //the index determines what button expect type this is
    private void ListenForClick(int index){

        //return
        Controller_Tutorial.instance.ShopButtonClick(index);
    }
}
