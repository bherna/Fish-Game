using UnityEngine;

public class Pets_InMainMenu : Pet_ParentClass
{
    
    private Vector3 coordPos;
    private float ability_vel = 3;
    private bool facingCorrectly = false;

    //run when we spawn this pet
    //save the spot this fish needs to swim to when we go to the pets ui panel
    public void SetCoords(Vector3 coords){

        coordPos = coords;
    }

    private new void Start(){
        base.Start();
    }

    // Update is called once per frame
    private new void Update()
    {
        //we dont need to pause this pet in game, since there is no pause menu
        //no base.update then


        switch(curr_PetState){

            case Pet_States.idle:
                IdleMode();
                break;
            case Pet_States.ability:
                AbilityMode();
                break;
            default:
                Debug.Log(gameObject.ToString()+" has no state to enter");
                break;
        }
    }


    //litterally just idle move but just stop after we reach coords
    private void AbilityMode(){

        var distance = Vector3.Distance(coordPos, transform.position);

        if(Mathf.Abs(distance) > targetRadius){
            
            updatePosition(coordPos, ability_vel);
        }

        else if(facingCorrectly){
            
            facingCorrectly = true;
            Vector2 right = new Vector2(transform.position.x+0.01f, transform.position.y);
            updatePosition(right, idle_velocity);
        }

    }


    //
    public void ToAbility(){
        curr_PetState = Pet_States.ability;
        facingCorrectly = false;
    }
    public void ToIdle(){
        curr_PetState = Pet_States.idle; 
        facingCorrectly = false;
    }


    //nothing for these, since we dont exist in the game level
    public override void Event_Init(Event_Type type, GameObject obj){}

    public override void Event_EndIt(Event_Type type){}
}
