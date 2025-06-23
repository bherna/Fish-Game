using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_Parent : MonoBehaviour
{

    [SerializeField] protected float timeTillTrashed = 3f;

    protected Rigidbody2D rb;



    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //jsut used to set up colliders, so they're not all out of sync from each other
    //base set up for use, here we do nothing, but put what ever code in the player if statement
    public virtual void OnMouseDown()
    {

        // ----------------------------------------- keep this in this exact order or we break stuff --------------------------
        if (Controller_EscMenu.instance.paused)
        {
            return;
        }

    }



    
    //this is for when drop touches the bottom of tank
    //normal logic is to destroy after _ seconds
    public virtual void OnTrashDrop()
    {
        //lock coin position
        rb.constraints = RigidbodyConstraints2D.FreezePosition;

        //countdown -> destroy
        IEnumerator coroutine = WaitToDes(timeTillTrashed);
        StartCoroutine(coroutine);
    }

    private IEnumerator WaitToDes(float waitTIme){

        yield return new WaitForSeconds(waitTIme);
        Destroy(gameObject);
    }
}
