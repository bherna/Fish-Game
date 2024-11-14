using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pet_ToggleButton : MonoBehaviour
{
    public PetNames petName;
    public bool selected = false; //are we currently selected

    public void Init(PetNames petname, bool selected){
        this.petName = petname;
        this.selected = selected;
        SetToggle(selected);
    }

    //on click button
    //check if we are true or false
    //return _____
    public void OnClickToggle(){

        bool tempSelected = !selected;

        //update selection
        //pass our temp selected into add or sub, the return bool should update button or not
        //update out button accordingly
        if(tempSelected){
            if(Controller_MainMenu.instance.Select_Add(petName)){
                //update color
                selected = tempSelected;
                SetToggle(selected);
            }
        }
        else{
            if(Controller_MainMenu.instance.Select_Sub(petName)){
                //update color
                selected = tempSelected;
                SetToggle(selected);
            }
        }
    }


    public void SetToggle(bool toggle){

        var butt = GetComponent<Button>();
        if(toggle){
            ColorBlock cb = butt.colors;
            cb.normalColor = Color.green;
            cb.selectedColor = Color.green;
            butt.colors = cb;
        }
        else{
            ColorBlock cb = butt.colors;
            cb.normalColor = Color.white;
            cb.selectedColor = Color.white;
            butt.colors = cb;
        }
        
    }
}
