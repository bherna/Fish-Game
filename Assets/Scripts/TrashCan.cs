using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{

    [SerializeField] Controller_Food controller_Food;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.tag == "Food"){
            controller_Food.TrashThisFood(other.gameObject);
        }
    }
}
