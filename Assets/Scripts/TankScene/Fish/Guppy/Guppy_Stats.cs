using System;
using System.Collections.Generic;
using UnityEngine;



public class Guppy_Stats : FishStats_ParentClass
{

    // --------------------------------- gubby script reference --------------------------------//
    private Guppy_SM guppy_SM;

    [SerializeField] List<Transform> sprite_meshList; //use only for sprite transperancy
    [SerializeField] Transform sprite_transform;        //use only for changing the entire size of guppy

    // --------------------------------- hunger related ----------------------------------------//
    private float stomach; //total seconds before fish will die of hunger
    protected float burnRate = 1; //keep at 1, just so have it reference as -1 unit per second
    private int hungryRange;
    private Color hungryColor = new Color(165,170,255);
    private Color fullColor = new Color(1,1,1);

    // --------------------------------- age related -------------------------------------------//  
    public int current_age_stage {get; private set; } = 0;
    public float amount_food_ate = 0;
    private int food_until_next_stage = 3;
    private float current_size = 0.3f; //also our guppy start size
    private float size_growth_speed = 0.15f;
    protected bool updateAge = true;

    


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        guppy_SM = GetComponent<Guppy_SM>();

        //update stomach
        StartStomach();

        //update sprite
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

            GuppyHungry();
        }
    }


    //at the start of game, we update our stomach cap to a random number between a range.
    //we do this to avoid having all guppys get hungry at same time, if player buys a bunch at once
    protected void StartStomach(){

        stomach = UnityEngine.Random.Range(15f, 22f);
        hungryRange = (int)stomach/2;
    }

    protected virtual void GuppyHungry(){

        //guppy is now hungry
        guppy_SM.GuppyToHungry();

        //announce to all pets that we are hungry
        Controller_Pets.instance.Annoucement_Init(Event_Type.guppyHungry, gameObject);

        //change sprite transparancy
        SetGuppyColor(hungryColor);
    }

    //used outside this script
    public virtual void GuppyEated(int foodValue){

        //return color to fish
        SetGuppyColor(fullColor); 

        //set our state to idle again
        guppy_SM.GuppyToIdle();

        //eating ages guppy
        Ate();

        //update fish stomach to add food value
        stomach += foodValue;
    }

    //the fish ate a burger, instead of food pellet
    public void GuppyBurgered(){

        //return color to fish
        SetGuppyColor(fullColor); 

        //set our state to idle again
        guppy_SM.GuppyToIdle();

        //-------burger specific ------
        //we skipp to next stage
        amount_food_ate = 0;
        Fish_Birthday(); //early birthday , if we are at max, birthday should just return false

        //reset stomach
        StartStomach();

    }


    //here where setting the guppy's hunger color
    //we don't really need to edit the transperency since this messes with the alpha cutoff, making
    //the guppy insvisiable

    private void SetGuppyColor(Color setColor){

        //for each sprite that is part of this fish
        //we have to check if its a skinned messrender, or a simple meshrender
        foreach(Transform sprite in sprite_meshList){


            var mesh = sprite.GetComponent<MeshRenderer>();
            if(mesh != null){

                mesh.material.SetColor("_Color", new Color(165,170,255));
                return;
            }
            
            var skindMesh = sprite.GetComponent<SkinnedMeshRenderer>();
            if(skindMesh != null){

                skindMesh.material.SetColor("_Color", new Color(165,170,255)); //no need to set alpha
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

    public void Fish_Birthday(){

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
        if(playSound){AudioManager.instance.PlaySoundFXClip(dieSoundClip, transform, 1f, 1f);}
        

        Destroy(gameObject);
    }
}
