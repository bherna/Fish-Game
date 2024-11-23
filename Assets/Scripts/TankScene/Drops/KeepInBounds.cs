using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepInBounds : MonoBehaviour
{

    private Rigidbody2D rb;
    private float kbForce = 0.7f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        
        //we're in the boudry, we need to head back
        if(other.gameObject.CompareTag("Boundry")){

            //set our velocity towards middle of tank
            Vector2 kb = (other.gameObject.transform.position - transform.position).normalized * kbForce;
            rb.AddForce(kb, ForceMode2D.Impulse);
            
        }
    }
}
