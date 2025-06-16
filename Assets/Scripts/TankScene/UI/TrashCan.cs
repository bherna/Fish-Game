using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.CompareTag("Food"))
        {
            other.gameObject.GetComponent<Drop_Food>().OnTrashDrop();
        }

        else if(other.gameObject.CompareTag("Money"))
        {
            other.gameObject.GetComponent<Drop_Money>().OnTrashDrop();
        }
    }
}
