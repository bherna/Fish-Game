using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;




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

public class EggPiece : MonoBehaviour
{

    public List<int> index; // this should be updated in controller_obj
                            //  : first index should always be the refereence index of this egg piece, alll other ints are attachments

    ///
    ///
    ///idle is the default state an egg piece starts as, since its not grabbed or dropped
    enum EggPiece_States {Idle, Grabbed, Dropped};
    private EggPiece_States eggState = EggPiece_States.Idle;
    private SpriteMask EggPieceMask; //visual representation of our index[].total (total cracks we are using)
    private SpriteRenderer EggSilhouette; //when onHover, this will show up 
    private Rigidbody2D rb;
    public CapsuleCollider2D crackCollider;
    private SortingGroup sortGroup; //this lets egg mask avoid masking other egg peices


    private void Start() {
        
        //set sprite renderers
        //egg silhouette
        EggSilhouette = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        ChangeTransparency(0);

        //eggpiece crack mask
        EggPieceMask = transform.GetChild(1).gameObject.GetComponent<SpriteMask>(); //update what crack we are using
        SetPetEgg();

        //rigidbody
        rb = GetComponent<Rigidbody2D>();

        //collider
        crackCollider = GetComponent<CapsuleCollider2D>();

        sortGroup = GetComponent<SortingGroup>();
        sortGroup.sortingOrder = index[0];

    }




    void OnCollisionStay2D(Collision2D other)
    {

        //this is to check if we are messing with another egg peice
        //grab collidee's egg script
        var ep_n = other.gameObject.GetComponent<EggPiece>();
        //
        if (ep_n != null)
        {
            OnEggContact(ep_n);
        }
    }

    void OnCollisionExit2D(Collision2D other){

        //grab collidee's egg script
        var ep_n = other.gameObject.GetComponent<EggPiece>();

        //if none, then return
        if(ep_n != null){
            ep_n.ChangeTransparency(0);
        }

        
    }





    private void OnEggContact(EggPiece ep_n)
    {
        //if we are hovering over an egg piece or dropping in
        switch (eggState)
        {
            case EggPiece_States.Idle:
                //do nothing, sincec egg peices can be colliding casually
                return;
            case EggPiece_States.Grabbed:
                //then make the collidee show its being hovered on, by highlighting complete egg as bg
                ep_n.ChangeTransparency(0.6f);
                break;

            case EggPiece_States.Dropped:
                //before each of these index checks, we do an edge case, 
                // if we are the ep_0, then don't check lower bound
                // if we are ep_max, then don't check upper bound


                //for each index in this dropped ep
                foreach (int i in index)
                {

                    //index[0] > 0, we check for 0 sincec thats the first ep_n possible
                    if ((i > 0 && ep_n.index.Contains(i - 1)) ||
                        (i < Controller_Objective.instance.final_obj - 1 && ep_n.index.Contains(i + 1))
                        )
                    {
                        //then combine
                        //by updating ep_n sprite + increment our index
                        ep_n.AttachSprite(index);

                        //send message to tutorial that we combined here too,
                        TutorialReaderParent.instance.EggPieceCombined();

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
    


    //does what the function is called, but only changes the egg silhouette alpha
    //used in onhover logic
    private void ChangeTransparency(float alphaValue)
    {

        EggSilhouette.color = new Color(0.95f, 0.49f, 0.54f, alphaValue); //set alpha to 0
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

        //update our egg to display all pieces we combined now
        UpdateCracks();

        //did we finish assembling?
        if(index.Count >= Controller_Objective.instance.final_obj){
            
            //send message to requirements
            Controller_Requirements.instance.StartDisplaying();
            //send referencec of self to objective controller, used in deleteing self later
            Controller_Objective.instance.SetFinalEggPiece(gameObject);
        }
    }


    private void UpdateCracks(){

        //update sprite
        index.Sort();
        string newSprite = string.Format("Eggs/EggCracks/Egg");
        string item = "";
        foreach(int i in index){
            item = item+"_"+i.ToString();
        }

        EggPieceMask.sprite = Resources.Load<Sprite>(newSprite+item);

        //update the collision box
        crackCollider.offset = EggColliders.crack1["Egg"+item].offset;
        crackCollider.size = EggColliders.crack1["Egg"+item].size;
        crackCollider.direction = EggColliders.crack1["Egg"+item].orientation;
        

    }


    //sets the crack type and what pet egg art to use
    public void SetPetEgg(){

        //update the acual pet egg we are using (not the crack type)
        string filePath = string.Format("Eggs/{0}_Full", LocalLevelVariables.GetUnlockPet_Name());
        Sprite petEgg = Resources.Load<Sprite>(filePath);
        transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = petEgg;

        //update the sprite crack to use
        UpdateCracks();
    }



    //----------------------------------------------------------  mouse drag related ---------------------------

    
    void OnTriggerStay2D(Collider2D other)
    {
        //onc
    }
    void OnTriggerExit2D(Collider2D other)
    {

    }



    //functino break downs
    void OnMouseDown() {

        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        //change to grabbed state
        eggState = EggPiece_States.Grabbed;

        //grab sound

    }
    void OnMouseDrag(){
        //now just follow the mouse position
        transform.position = (Vector2)Controller_Player.instance.mousePos - crackCollider.offset;

    }
    
    //this is used as the main funciton on mouseup
    void OnMouseUp() {

        rb.gravityScale = 1;
        rb.constraints = RigidbodyConstraints2D.None;
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
