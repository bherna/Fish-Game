
using UnityEngine.EventSystems;
using UnityEngine;
using Assests.Inputs;


public enum Enemy_States {idle, attack, stunned, ability};

public class Enemy_ParentClass : Fish_ParentClass_Movement 
{
    [SerializeField] protected AudioClip damageSoundClip;
    [SerializeField] protected AudioClip diedSoundClip;
    [SerializeField] protected GameObject gem; //on die we drop this
    protected Enemy_States curr_EnemyState;
    

    //targets
    protected Transform currFishTarget;

    
    //stats
    //health should be in # of clicks
    protected int curr_health = 6; 
    protected const float kbForce_player = 0.7f; //player related, for tank kb just keep at a vector of 1;
    protected const float kbForce_stunned = 1.2f; //when an enemy is stunned, we use this one instead (since player can just spam click)


    //particles
    [SerializeField] protected ParticleSystem stun_particle;



    protected new void Start()
    {
        base.Start();

        //references

        //idle target set
        NewRandomIdleTarget_Tank();

        //attack mode target set
        SetTargetFish(Controller_Fish.instance.GetRandomFish());

        //start in attack mode if possible
        curr_EnemyState = Enemy_States.attack;

        //all enemy fish will be dynamic in its movement
        //a select few will be front facing, so we'll have to update them per enemy
        IStatic = false;
        

    }

    //---------------------------------------------------- standard enemy fish code to function -------------------------------------
    public void SetTargetFish(Transform newFishTarget){

        currFishTarget = newFishTarget;
    }



    protected void TakeDamage(int damage){

        curr_health -= damage;
        if(curr_health <= 0){
            Died();
        }
    }



    //fish died
    //dont want to flat out destroy object
    protected virtual void Died(){

        //remove the enemy from list
        Controller_Enemy.instance.CloserToWaveEnded();

        //play sound
        AudioManager.instance.PlaySoundFXClip(diedSoundClip, transform, 1f, 1f);

        //drop gem
        Instantiate(gem, transform.position, Quaternion.identity);

        //die
        Destroy(gameObject);
    }
    


    ///--------------------------------------- collision related code here -----------------------------------------------------------

    //2nd-dairy colider, used for interacting with tank(oncollisionEnter2d)
    public virtual void On_TankEnter(Collider2D other)
    {

    }

    //whenever enemy is casually inside boundry, (oncollisionStay2d)
    public virtual void On_TankStay(Collider2D other)
    {
        if (other.gameObject.CompareTag("Boundry"))
        {

            //set our velocity towards middle of tank
            //(IF CHANGED, UPDATE THIS IN CHILD CLASSES)
            Vector2 kb = (other.gameObject.transform.position - transform.position).normalized;
            rb.velocity = kb; 

        }
    }
    
    public virtual void On_TankExit(Collider2D other)
    {

    }


    //2nd-dairy collider, used for interactin with player (clicks)
    //Purpose of seperating collision and onpointerclick is to be able to dynamically edit collision box of tank interaction vs player interaction
    //this isn't the normal onplayerclick method, this is our own version to avoid calling it here (should be called from {enemyname}_collider)
    public virtual void On_PlayerClick()
    {
        //if the game is paused, return
        if (Controller_EscMenu.instance.paused) { return; }


        //create gun particle
        Controller_Player.instance.Run_GunParticle();

        //knockback
        Vector2 kbVector = ((Vector2)transform.position - CustomVirtualCursor.GetMousePosition_V2()).normalized;
        rb.AddForce(kbVector * kbForce_player, ForceMode2D.Impulse);


        //damage
        TakeDamage(Controller_Player.instance.Get_GunDamage());

    }


    //since we need one for player collision
    public virtual void On_PlayerEnter(Collider2D collision) { }
    public virtual void On_PlayerStay(Collider2D collision) { }
    public virtual void On_PlayerExit(Collider2D collision) { }




    //this isn't really 'collision' per say, but used when collideing with pets 
    //specifiicallly with cherry and player counters (so far)
    //functions purpose is to stun this enemy
    //we can pass an integer to update the number of seconds the stun can last
    public virtual void OnStunned(int numOfSeconds)
    {
        //for every enemy, we want to play stunned visual
        //so instantiate and update duration
        var particle = Instantiate(stun_particle, transform);
        Destroy(particle, numOfSeconds);
    }








    ///------------------------------------------------------ scene ui code not really important to functionality ----------------------------

    protected new void OnDrawGizmosSelected()
    {

        switch (curr_EnemyState)
        {
            case Enemy_States.idle:
                //curr idle target
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(idleTarget, 0.5f);
                break;
            case Enemy_States.attack:
                if (currFishTarget == null) { return; }
                //current fish target
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(currFishTarget.transform.position, 0.5f);
                break;
        }

    }


}
