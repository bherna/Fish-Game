using System.Collections;
using UnityEngine;
using TMPro;

public class TextPopUp : MonoBehaviour
{

    private TextMeshPro uiText;

    //call when we get first created
    public void UpdateText(string text){

        uiText = GetComponent<TextMeshPro>();

        uiText.text = text;
    }


    public void DestroySelf(){
        Destroy(gameObject);
    }
}
