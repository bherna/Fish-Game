
using UnityEngine;


public class Starfish_P_Collider : MonoBehaviour
{


    //-------------------------------------------------------- collider specific stuf ------------------------------------------------------------

    CapsuleCollider2D capCollider;


    //start state like a unit circle
    private ColliderDem attack_col = new ColliderDem(new Vector2(0.4f, 0), new Vector2(2.4f, 1.4f), CapsuleDirection2D.Horizontal);

    //a circle encompassing the enemy 
    private ColliderDem idle_col = new ColliderDem(new Vector2(0, 0), new Vector2(1.5f, 1.5f), CapsuleDirection2D.Horizontal);

    ////-------------------------------------------------------- other //-------------------------------------------------------
    private Starfish_SM starfish_SM;

    /// 


    // Start is called before the first frame update
    protected void Start()
    {

        starfish_SM = GetComponent<Starfish_SM>();

        capCollider = GetComponent<CapsuleCollider2D>();
        SetOrientation(Enemy_States.idle, 0);

    }




    //everytime we need to update our collision orientation
    //orientation variable is used  to set the angle our collider should be heading towards, used in making it easier for player to counter enemy
    public void SetOrientation(Enemy_States state, int orientaion_angle)
    {


        switch (state)
        {

            //only need to do somthing on attack, i think
            case Enemy_States.attack:

                capCollider.offset = attack_col.offset;
                capCollider.size = attack_col.size;
                transform.Rotate(0, 0, orientaion_angle);
                break;


            //everying else, reset to regular collision
            default:
                capCollider.offset = idle_col.offset;
                capCollider.size = idle_col.size;
                transform.rotation = Quaternion.identity;
                break;

        }


    }


    
    //this isn't the normal onplayerclick method, this is our own version to avoid calling it here (should be called from {enemyname}_collider)
    public void OnMouseDown()
    {
        starfish_SM.On_PlayerClick();
    }


}
