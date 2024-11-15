using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Stats : FishStats_ParentClass
{

    // --------------------------------- gubby script reference --------------------------------//
    private Guppy_SM guppy_SM;

    [SerializeField] List<Transform> sprite_transparency; //use only for sprite transperancy
    [SerializeField] Transform sprite_transform;        //use only for changing the entire size of guppy

    // --------------------------------- hunger related ----------------------------------------//
    private float stomach;
    private const int startStomach = 20;//total seconds before fish dies of hunger
    private float burnRate = 1; //per second (could be changed for other level types "fever")
    private int hungryRange = startStomach/2; 

    // --------------------------------- age related -------------------------------------------//  
    public int current_age_stage {get; private set; } = 0;
    public float amount_food_ate = 0;
    private int food_until_next_stage = 3;
    private float current_size = 0.3f; //also our guppy start size
    private float size_growth_speed = 0.15f;
    private bool updateAge = true;

    


    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        guppy_SM = GetComponent<Guppy_SM>();

        stomach = startStomach;
        ChangeGuppySize(); //to currently held size

        
    }

    // Update is called once per frame
    void Update()
    {
        //burn stomach
        stomach -= burnRate * Time.deltaTime;

        //check if fish starved to death
        if(stomach <= 0){
            Died();
        }
        //if guppy became hungry
        else if(stomach < hungryRange && guppy_SM.guppy_current_state != Guppy_States.hungry){

            //guppy is now hungry
            guppy_SM.GuppyToHungry();

            //announce to all pets that we are hungry
            Controller_Pets.instance.Annoucement_Init(Event_Type.food, gameObject);

            //change sprite transparancy
            ChangeTransparency(false);

            //also check if this was a tutorial push
            Controller_Tutorial.instance.TutorialClick(ExpectType.Fish_Hungry);
            
        }

    }

    //used outside this script
    public void FishEated(int foodValue){

        //return color to fish
        ChangeTransparency(true); 

        //set our state to idle again
        guppy_SM.GuppyToIdle();

        //eating ages guppy
        Ate();

        //update fish stomach to add food value
        stomach += foodValue;

        //check if this feeding was for fish to push tutorial
        Controller_Tutorial.instance.TutorialClick(ExpectType.Fish_Feed);
    }

    //the fish ate a burger, 
    public void FishBurgered(){

        //return color to fish
        ChangeTransparency(true); 

        //set our state to idle again
        guppy_SM.GuppyToIdle();

        //-------burger specific ------
        //we skipp to next stage
        amount_food_ate = 0;
        Fish_Birthday(); //early birthday , if we are at max, birthday should just return false

        //update fish stomach to start stomach value
        stomach = startStomach;

    }



    private void ChangeTransparency(bool setFullAlpha){

        //we have to check if its a skinned messrender, or a simple meshrender
        foreach(Transform sprite in sprite_transparency){

            try{
                //first we try simple messrender
                if(setFullAlpha){sprite.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1,1,1,1));}
                else{sprite.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1,1,1,0.5f));}
            }
            catch(MissingComponentException ){
                //else we use skinned mesh
                if(setFullAlpha){sprite.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Color(1,1,1,1));}
                else{sprite.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", new Color(1,1,1,0.5f));}
            } 
        }

        
    }

    private void ChangeGuppySize(){

        sprite_transform.transform.localScale = new Vector3(current_size, current_size, current_size);
    }


    private void Ate(){
        if(updateAge){

            //update age of fish
            amount_food_ate += 1;

            if(amount_food_ate >= food_until_next_stage){
                Fish_Birthday();
            }
        }
    }

    private void Fish_Birthday(){

        //if current age is not final stage
        if(current_age_stage < Controller_Fish.instance.GetFishStages().Count-1){
            
            //update 
            current_age_stage += 1;
            current_size += size_growth_speed;
            ChangeGuppySize();

            //reset
            amount_food_ate = 0;
            
        }
        else{
            //we are done aging
            updateAge = false;
        }
    }


    public override void TakeDamage(int damage){

        health -= damage;

        if(health <= 0){

            Died();

        }
    }

    public override void Died(bool playSound = true){

        //removes self from the list of current fish known to the fish controller
        Controller_Fish.instance.RemoveFish(gameObject);
        
        //play die sound
        if(playSound){AudioManager.instance.PlaySoundFXClip(dieSoundClip, transform, 1f);}
        

        Destroy(gameObject);
    }
}
