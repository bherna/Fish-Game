using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.UI;



//each egg piece is numberd
//first egg purchase ==> eggPiece_0
//second ... ==> eggpiece_1
//...

//ep_0 is the base, so
//when ep_1 is being held by player and collides with ep_0, attach
//when ep_2 is held ... collides with ep_1, attach
//ie, we HAVE  to attach ep_1 to ep_0 first, then ep_2, to ep_1_0

//when all ep_s are attached, we end level
//11111111111111


//ep_0 is the base, so
//we always keep ep_0 over any other ep_n, for scaliabilty
//ep_0 's sprite will change when ever a new egg piece is attached.

public class EggPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public List<int> index; // this should be updated in controller_obj
                            //this first index should always be the refereence index of this egg piece, alll other ints are attachments

    ///
    ///
    ///null is the default state an egg piece starts as, since its not grabbed or dropped
    enum EggPiece_States {Null, Grabbed, Dropped};
    private EggPiece_States eggState = EggPiece_States.Null;

    public Sprite newSprite; // used in accepting new sprite from controller;
    private SpriteRenderer sr;


    private void Start() {
        
        //get sprite renderer
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = newSprite;
    }


    private void OnTriggerStay2D(Collider2D other){


        
        //if the current eggpeice held is greater than ep_0, then we have a chance to enter this
        //ep_0 should not enter this
        var ep_n = other.gameObject.GetComponent<EggPiece>();
        if(
            eggState == EggPiece_States.Dropped &&
            ep_n.index.Contains(index[0]-1))
        {

            //then combine
            //by updating ep_o sprite + increment our index
            ep_n.AttachSprite(index);
            //delete self
            Destroy(gameObject);


        }
    }


    //if we are ep_0, then we have access to these functins (but other ep-s will run them)
    public void AttachSprite(List<int> new_indexs){

        //attach all new ep_indexs to the lower index
        foreach(int i in new_indexs){
            index.Add(i);
        }

        //update sprite
        //idk yet
        Debug.Log("COMbinesddeddd");
    }








    //----------------------------------------------------------  mouse drag related ---------------------------
    public void OnBeginDrag(PointerEventData data){

        //change to grabbed state
        eggState = EggPiece_States.Grabbed;

        //grab sound

    }
    public void OnDrag(PointerEventData data){

        //now just follow the mouse position
        transform.position = Controller_Player.instance.mousePos;

    }
    public void OnEndDrag(PointerEventData data){
        //drop state
        eggState = EggPiece_States.Dropped;


        //if we ddint collide, then just go into null state again, else bugg'n
        IEnumerator co = IdleReturn();
        StartCoroutine(co);
    }
    private IEnumerator IdleReturn() {

        yield return new WaitForSeconds(0.1f);
        eggState = EggPiece_States.Null;
    }

    
}
