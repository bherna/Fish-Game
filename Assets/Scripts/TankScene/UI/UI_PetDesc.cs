using UnityEngine;
using TMPro;


public class UI_PetDesc : MonoBehaviour
{


    [SerializeField] TextMeshProUGUI description;

    
    // Start is called before the first frame update
    void Start()
    {
        //grab the pet unlocked description and set text on ui
        description.text = PetDescription.GetPetDesc(LocalLevelVariables.GetUnlockPet_Enum());

    }


    //for the continue Button
    public void Continue(){


        //activate post game
        Controller_Objective.instance.ActivatePostEndGameUI();

        //close this UI by disableing this
        gameObject.SetActive(false);
    }

}
