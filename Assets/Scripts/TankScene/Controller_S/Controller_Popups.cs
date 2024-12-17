
using UnityEngine;





//this class is reference when we want to create a new pop up show up on the screen



public class Controller_PopUp : MonoBehaviour
{


    //
    [SerializeField] GameObject textPopUp;


    //single ton this class
    public static Controller_PopUp instance {get; private set; }

    private void Awake() {
        
        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }




    //all we need is other  classes to reference the popup creation
    public void CreatePopUp(string text){

        var popup = Instantiate(textPopUp);
        popup.GetComponent<TextPopUp>().UpdateText(text);
        popup.transform.SetParent(transform);
        popup.transform.position = Input.mousePosition + Vector3.right;
    }
}
