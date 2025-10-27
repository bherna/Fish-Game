using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings_Slider_Volume : Settings_Slider_Parent
{

    [SerializeField] VolumeType volumeType = VolumeType.Null; //this should not be null, update in the inpsector
    public float newPercent { get; private set; } = 1; //out of 100%, yu know


    //with mouse speed we just need to update our mouse base variable
    public override void UpdateSliderValueText(float value)
    {
        //cant use base versoin cause we using percentage
        sliderValueText.SetText(value.ToString("P0"));
        //and update our value to be new percent, but its not set yet
        newPercent = value;
    }

    protected override void OnEnable()
    {
        base.OnEnable();


        //set our initial slider values.... were this in hrtz
        //our min and max slider values
        slider.minValue = 0;
        slider.maxValue = 1;
        //next our main value == base speed
        slider.value = newPercent;


        //now we set our display text value => to whole  percentage value
        sliderValueText.SetText(slider.value.ToString("P0"));
    }


    //when we apply, we send that information to the sound mixer  manager
    public override void ApplySettings()
    {
        //finally apply new percent
        //Debug.Log(string.Format("vol-Type: {0}",volumeType.ToString()));
        Controller_SoundMixerManager.instance.SetNewVolume(volumeType, newPercent);
    }
}
