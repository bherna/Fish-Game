using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Color hungryColor = new Color(0.63f, 0.66f, 1f);
    private Color fullColor = new Color(1,1,1);

    // --------------------------------- age related -------------------------------------------//  
    public int curr_ageStage {get; private set; } = 0; //used in getting list indexes below
    private int[] foodForNext_ageStage = {3, 5, 60}; //to teenfish, to adultfish, to gemfish                             
    public float curr_foodAte = 0; //so far
    private float current_size = 0.3f; //also our guppy start size
    private float[] spriteGrowthFor_ageStage = {0.15f, 0.15f, 0}; //how much does a guppy grow by, indexing is same as food till next stage
    protected bool updateAge = true; //if true, then we arn't at max age yet

    


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        guppy_SM = GetComponent<Guppy_SM>();

        //update stomach
        StartStomach();

        //set sprite size
        sprite_transform.transform.localScale = new Vector3(current_size, current_size, current_size);

        
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

        stomach = UnityEngine.Random.Range(15f, 17f);
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
        curr_foodAte = 0;
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

                mesh.material.SetColor("_BaseColor", setColor);
                
            }
            else{

                var skindMesh = sprite.GetComponent<SkinnedMeshRenderer>();
                if(skindMesh != null){

                    skindMesh.material.SetColor("_BaseColor", setColor); 
                }
            }
            
            
        }

        
    }

    private IEnumerator ChangeGuppySize(float newSizeDelta){
        Debug.Log("Started grow");

        var newSize = current_size + newSizeDelta;
        //now loop
        while(current_size < newSize){
            Debug.Log("growing...");
            //update size
            current_size += Time.deltaTime * 2;
            sprite_transform.transform.localScale = new Vector3(current_size, current_size, current_size);

            //then we wait, so its not instant growth
            yield return new WaitForSeconds(0.1f);
        }

        //update size to be EXACTLY the new size
        //since the float gets gross
        sprite_transform.transform.localScale = new Vector3(newSize, newSize, newSize);

    }


    private void Ate(){
        if(updateAge){

            //increment food ate
            curr_foodAte += 1;
            
            //check if this is enough to go to next ageStage
            if(curr_foodAte >= foodForNext_ageStage[curr_ageStage]){
                Fish_Birthday();
            }
        }
    }

    public void Fish_Birthday(){

        //this is a check in case we are freely giving out birthdays to guppys
        if(!updateAge){return;} 

        //update age
        //but before we increment index, we update other variables, since they are dependnt on curr index
        //sprite
        //
        StartCoroutine(ChangeGuppySize(spriteGrowthFor_ageStage[curr_ageStage])); 
        
        //reset
        curr_foodAte = 0;

        //now we increment
        curr_ageStage += 1; //one year older bro

        //if we reached final stage, then stop updating age
        if(curr_ageStage > foodForNext_ageStage.Count()){
        
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
