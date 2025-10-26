using System.Collections;
using System.Collections.Generic;
using Assests.Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Settings_Slider_Parent : Settings_Apply_Parent
{

    //for a slider we are going to need:
    [SerializeField] protected Slider slider;
    [SerializeField] protected TMP_Text sliderValueText;


    private void Resest()
    {
        slider = GetComponent<Slider>();
        sliderValueText = GetComponent<TMP_Text>();

    }

    //since this a parent class, we dont actually put anything here, unless we plan on doing it on every slider to exist
    //when this function is called we need to update what this changes in the game, aside from the displayed text.
    public virtual void UpdateSliderValueText(float value)
    {
        sliderValueText.SetText(value.ToString("N0"));

        //more here
    }

    protected virtual void OnEnable()
    {
        //if either of the slider components are missing, we dont set
        if (slider == null || sliderValueText == null)
        {
            return;

        }

        //Register Button Events
        slider.onValueChanged.AddListener(UpdateSliderValueText);

    }

    void OnDisable()
    {
        //Un-Register Button Events
        slider.onValueChanged.RemoveListener(UpdateSliderValueText);
    }

    public override void ApplySettings()
    {
        Debug.Log("this setting does not have a setting to apply...");
    }
}
