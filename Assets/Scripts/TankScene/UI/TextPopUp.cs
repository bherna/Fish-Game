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


    //on true, we want to re-enable this
    //
    public void SetTextAlpha(bool val){

        if(val){    
            
            //re-enable animator
            transform.GetChild(0).GetComponent<Animator>().enabled = true;
            //it'll do the rest of the work for us
        }
        else{

            //disable animator, else it overrides our alpha
            transform.GetChild(0).GetComponent<Animator>().enabled = false;
            uiText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Color temp = uiText.color;
            temp.a = 0;
            uiText.color = temp;
            uiText.ForceMeshUpdate();
        }
        

    }

}
