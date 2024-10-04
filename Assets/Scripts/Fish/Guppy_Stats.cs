using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guppy_Stats : Fish_Stats
{

    // --------------------------------- gubby script reference --------------------------------- 
    private Guppy_SM guppy_SM;
    private Fish_Age fish_Age;

    [SerializeField] List<Transform> sprite_transparency; //fish sprites

    // --------------------------------- hunger related --------------------------------- 
    private float stomach;
    private const int startStomach = 20;//total seconds before fish dies of hunger
    private float burnRate = 1; //per second (could be changed for other level types "fever")
    private int hungryRange = startStomach/2; 


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        guppy_SM = GetComponent<Guppy_SM>();
        fish_Age = GetComponent<Fish_Age>();

        stomach = startStomach;
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

            //change sprite transparancy
            ChangeTransparency(false);

            //also check if this was a tutorial push
            Controller_Tutorial.instance.TutorialClick(Expect_Type.Fish_Hungry);
            
        }

    }


    public void FishEated(int foodValue){

        //return color to fish
        ChangeTransparency(true);

        //set our state to idle again
        guppy_SM.GuppyToIdle();

        //eating ages guppy
        fish_Age.Ate();

        //update fish stomach to add food value
        stomach += foodValue;

        //check if this feeding was for fish to push tutorial
        Controller_Tutorial.instance.TutorialClick(Expect_Type.Fish_Feed);
    }



    private void ChangeTransparency(bool setFullAlpha){

        //we have to check if its a skinned messrender, or a simple meshrender
        foreach(var sprite in sprite_transparency){

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


    



}
