using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_Dialogue : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] string[] lines;
    
    [SerializeField] Timer timer;
    [SerializeField] Mask tab_ui_mask;

    private int index;    
    private float textSpeed = 0.05f;
    
    // Start is called before the first frame update
    void Start()
    {
        //disable shop ui
        tab_ui_mask.enabled = true; //when true it makes it unclickable

        textUI.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){

            if(textUI.text == lines[index]){
                NextLine();
            }
            else{
                StopAllCoroutines();
                textUI.text = lines[index];
            }
        }
    }

    private void StartDialogue(){
        index = 0;
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine(){
        //for each char 1 by 1
        foreach( char c in lines[index].ToCharArray()){

            textUI.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }


    private void NextLine(){
        if (index < lines.Length - 1){
            index++;
            textUI.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else{

            //enable timer
            timer.StartTimer();

            //enable ui shop
            tab_ui_mask.enabled = false;

            //disable this object
            gameObject.SetActive(false);
        }
    }


    
}
