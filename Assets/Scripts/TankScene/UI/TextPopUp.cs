using System.Collections;
using UnityEngine;
using TMPro;

public class TextPopUp : MonoBehaviour
{

    private TextMeshProUGUI uiText;

    
    //call when we get first created
    public void UpdateText(string text){

        uiText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        uiText.text = text;
    }

}
