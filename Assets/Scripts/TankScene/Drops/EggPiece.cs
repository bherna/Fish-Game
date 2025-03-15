using System.Collections;

using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.UI;



//each egg piece is numberd
//egg purchase ==> eggPiece_0
//second ... ==> eggpiece_2
//...

//ep_0 is the base, so
//when ep_1 is being held by player and collides with ep_0, attach
//when ep_2 is held ... collides with ep_1, attach
//ie, we HAVE  to attach ep_n+1 to ep_n or ep_n-1 to ep_n

//when all ep_'s are attached, we complete the egg puzzel
//11111111111111


//ep_0 is the base, so
//we always keep ep_0 over any other ep_n, for comformaty
//ep_0 's sprite will change when ever a new egg piece is attached.

public class EggPiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public List<int> index; // this should be updated in controller_obj
                            //  : first index should always be the refereence index of this egg piece, alll other ints are attachments

    ///
    ///
    ///idle is the default state an egg piece starts as, since its not grabbed or dropped
    enum EggPiece_States {Idle, Grabbed, Dropped};
    private EggPiece_States eggState = EggPiece_States.Idle;

    public Sprite newSprite; // used in accepting new sprite from controller;
    private SpriteRenderer sr;
    private Rigidbody2D rb;


    private void Start() {
        
        //get sprite renderer
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = newSprite;

        rb = GetComponent<Rigidbody2D>();

    }


    void OnCollisionStay2D(Collision2D other)
    {
        //grab collidee's egg script
        var ep_n = other.gameObject.GetComponent<EggPiece>();

        //if none, then return
        if(ep_n == null){
            return;
        }


        //if we are hovering over an egg piece or dropping in
        switch(eggState){
            case EggPiece_States.Idle:
                //do nothing, sincec egg peices can be colliding casually
                return;
            case EggPiece_States.Grabbed:
                //then make the collidee show its being hovered on, by highlighting complete egg as bg
                break;

            case EggPiece_States.Dropped:
                //before each of these index checks, we do an edge case, 
                // if we are the ep_0, then don't check lower bound
                // if we are ep_max, then don't check upper bound


                //for each index in this dropped ep
                foreach(int i in index){
                    
                    //index[0] > 0, we check for 0 sincec thats the first ep_n possible
                    if((i > 0 && ep_n.index.Contains(i-1)) ||
                        (i < Controller_Objective.instance.final_obj-1 && ep_n.index.Contains(i+1))
                        ){
                        //then combine
                        //by updating ep_n sprite + increment our index
                        ep_n.AttachSprite(index);
                        //delete self
                        Destroy(gameObject);
                        break;//incase we dont stop on destroy
                    }
                }
                break;
                
            default:
                Debug.Log("No state possible");
                break;
        }
        
            


        
    }


    //this function will only be called on collided egg peices
    public void AttachSprite(List<int> new_indexs){

        //attach all new ep_indexs to the lower index
        //if new ep_index is lower than index[0], then make that our new ep_base
        if(new_indexs[0] < index[0]){
            //new base
            index.Insert(0, new_indexs[0]);
            new_indexs.RemoveAt(0);
        }

        foreach(int i in new_indexs){
            index.Add(i);
        }

        //update sprite
        index.Sort();
        string newSprite = string.Format("EggsSprites/{0}/{0}", LocalLevelVariables.GetUnlockPet_Name());
        foreach(int i in index){
            newSprite = newSprite+"_"+i.ToString();
        }

        sr.sprite = Resources.Load<Sprite>(newSprite);


        //did we finish assembling?
        if(index.Count >= Controller_Objective.instance.final_obj){
            Controller_Objective.instance.LevelComplete();
        }

    }








    //----------------------------------------------------------  mouse drag related ---------------------------
    public void OnBeginDrag(PointerEventData data){
        rb.gravityScale = 0;
        //change to grabbed state
        eggState = EggPiece_States.Grabbed;

        //grab sound

    }
    public void OnDrag(PointerEventData data){
        //now just follow the mouse position
        transform.position = Controller_Player.instance.mousePos;

    }
    public void OnEndDrag(PointerEventData data){
        rb.gravityScale = 1;
        //drop state
        eggState = EggPiece_States.Dropped;


        //if we ddint collide, then just go into null state again, else bugg'n
        IEnumerator co = IdleReturn();
        StartCoroutine(co);
    }
    private IEnumerator IdleReturn() {

        yield return new WaitForSeconds(0.1f);
        eggState = EggPiece_States.Idle;
    }

    
}
