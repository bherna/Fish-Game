using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Pet_ToggleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
            if(Controller_PetMenu.instance.Select_Add(petName)){
                //update color
                selected = tempSelected;
                SetToggle(selected);
            }
        }
        else{
            if(Controller_PetMenu.instance.Select_Sub(petName)){
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



    public void OnPointerEnter(PointerEventData eventData){
        
        string petNameString = Enum.GetName(typeof (PetNames), petName);
        ToolTip.ShowToolTip(petNameString);
    }

    public void OnPointerExit(PointerEventData eventData){
        ToolTip.HideToolTip();
    }
}
