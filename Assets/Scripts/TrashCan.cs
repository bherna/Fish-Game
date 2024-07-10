using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.CompareTag("Food"))
        {
            Controller_Food.instance.TrashThisFood(other.gameObject);
        }

        if(other.gameObject.CompareTag("Money"))
        {
            other.gameObject.GetComponent<Coin>().OnTrashCoin();
        }
    }
}
