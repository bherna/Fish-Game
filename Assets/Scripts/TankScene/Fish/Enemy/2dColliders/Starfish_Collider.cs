
using UnityEngine;


public class Starfish_Collider : Enemy_Collider_ParentClass
{


    //-------------------------------------------------------- collider specific stuf ------------------------------------------------------------

    CapsuleCollider2D capCollider;


    //start state like a unit circle
    private ColliderDem attack_col = new ColliderDem(new Vector2(0.4f, 0), new Vector2(2.4f, 1.4f), CapsuleDirection2D.Horizontal);

    //a circle encompassing the enemy 
    private ColliderDem idle_col = new ColliderDem(new Vector2(0, 0), new Vector2(1.5f, 1.5f), CapsuleDirection2D.Horizontal);

    ////-------------------------------------------------------- other //-------------------------------------------------------



    // Start is called before the first frame update
    protected override void Start()
    {
        //this should work for all collilder types
        enemy_ParentClass = transform.parent.GetComponent<Starfish_SM>();

        //specific to starfish player collider
        if (colliderType == ColliderType.Player)
        {
            capCollider = GetComponent<CapsuleCollider2D>();
            SetOrientation(Enemy_States.idle, 0);
        }
        //else we don't do this and we return

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


    
    


}
