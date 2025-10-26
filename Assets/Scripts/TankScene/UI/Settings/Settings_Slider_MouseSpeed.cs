using System.Collections;
using System.Collections.Generic;
using Assests.Inputs;
using UnityEngine;

public class Settings_Slider_MouseSpeed : Settings_Slider_Parent
{


    //this variable hold our next mouse speed to set
    // the player needs to press the accept button to make this new value the main player mouse speed
    //else its kept to what is current/ no change
    int newMouseSpeed = 2500;




    //with mouse speed we just need to update our mouse base variable
    public override void UpdateSliderValueText(float value)
    {
        base.UpdateSliderValueText(value);
        
        //set our speed to the value given
        newMouseSpeed = (int)value;
        //Debug.Log("new value: " + newMouseSpeed);
    }

    protected override void OnEnable()
    {
        base.OnEnable();


        //set our initial slider values....
        //our min and max slider values
        slider.minValue = 500;
        slider.maxValue = 3000;
        //next our main value == base speed
        slider.value = CustomVirtualCursor.cursorSpeed_playerSet;


        //now we set our display text value
        sliderValueText.SetText(slider.value.ToString("N0"));
    }




    //player clicked Apply settings button
    public override void ApplySettings()
    {
        //Debug.Log("applied new speed");

        //apply new base speed
        CustomVirtualCursor.UpdateMouseSpeed(newMouseSpeed);

        //then update the current speed by also checking to see if we have any debuffs attached
        //entering 0 lets us just update, since that doesn't change the debuff value.
        Controller_Player.instance.GiveMouseSpdStatusEffect(0);
    }

    
}
