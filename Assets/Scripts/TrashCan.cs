using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{

    [SerializeField] Controller_Food controller_Food;

    // Start is called before the first frame update
    

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.tag == "Food"){
            controller_Food.TrashThisFood(other.gameObject);
        }

        if(other.gameObject.tag == "Money"){
            Destroy(other.gameObject);
        }
    }
}
