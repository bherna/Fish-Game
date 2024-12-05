using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPopUp_Destroy : MonoBehaviour
{
    //destroy parent obj
    //yea
    public void DestroyParent(){
        Destroy(transform.parent.gameObject);
    }
}
