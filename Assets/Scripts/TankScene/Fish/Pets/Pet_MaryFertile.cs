using System;
using Steamworks;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 
//    Desc: A pregnat fish lady named Mary Fertile, who loves to have as many children as possible
//    Ability:  Every _ number of seconds Mary will spawn in a new guppy into the tank
//              The guppy itself is just a normal guppy, nothing crazy,
//              - Mary does not spawn guppys during enemy wave mode
//    Rarity: Level Questing
//
/// </summary>
/// 


public class Pet_MaryFertile : Pet_ParentClass
{

    [SerializeField] GameObject guppy_prefab; //for spawning 
    [SerializeField] Animator animator; 
    [SerializeField] SkinnedMeshRenderer eye_meshRender; 
    [SerializeField] ParticleSystem ps_sweating;

    // Event_Type event_type = Event_Type.enemyWave;
    private Material[] eyes; //for updating eye sprites

    private float curr_secBefore = 0;
    private const float max_secBefore = 17f; //#_ seconds before pregnat
    private float curr_secAfter = 0;
    private const float max_secAfter = 8f; //#_ seconds pregnat


    // -- ------------------ depression related - ---------------------
    private const int DeadGuppyThreshold = 5;
    private int guppysDeathToll = 0;

    private GameObject foodTarget;
    private const int FoodAteThreshold = 10;
    private int foodsEaten = 0;
    private int hungry_velocity = 2;




    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();

        //set eyes list
        eyes = eye_meshRender.materials;
        Material[] first = new Material[1];
        Array.Copy(eyes, first, 1);
        eye_meshRender.materials = first;

        //update particle system length
        ps_sweating.Stop();
        var temp_main = ps_sweating.main;
        temp_main.duration = max_secAfter; //gues this is a pointer to ps_sweating duration


        //we also start in stage1, not idle
        curr_PetState = Pet_States.stage1;
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update(); //incase

        //using this as just movement, since all she does is swim around the tank
        IdleMode(); 


        switch (curr_PetState)
        {
            //incase we somehow start with idle
            case Pet_States.idle:
            case Pet_States.stage1:

                Stage1Mode();
                break;

            case Pet_States.stage2:

                Stage2Mode();
                break;

            case Pet_States.ability:
                AbilityMode();
                break;

            case Pet_States.depressed:
                DepressedMode();
                break;

            default:
                Debug.Log("should NOT be here");
                break;
        }

        
        
    }

    private void Stage1Mode()
    {
        //keep adding untill mary becomes pregnant
        curr_secBefore += Time.deltaTime;

        //if we can become preg
        if (curr_secBefore >= max_secBefore)
        {
            //mary is now pregnant
            animator.SetBool("isPreg", true);
            eye_meshRender.material = eyes[1]; //1 == closed eye 'blink'
            ps_sweating.Play(); //play sweating particles

            //enter next state
            curr_PetState = Pet_States.stage2;
        }
    }

    private void Stage2Mode()
    {
        //same counter setup but now mary is pregnant
        curr_secAfter += Time.deltaTime;

        if (curr_secAfter >= max_secAfter)
        {
            //enter next stage
            curr_PetState = Pet_States.ability;
        }
    }


    private void AbilityMode()
    {
        //if we are in enemy wave, return, since we don't want to spawn yet
        if (Controller_Enemy.instance.currently_in_wave) { return; }

        //if we are down here, 
        //  - we are ready to spawn guppy
        //  - we are not in enemy wave

        //BABY TIMEEEEEEEEEEEEE
        Controller_Fish.instance.SpawnFish(guppy_prefab, transform.position);
        animator.SetBool("isPreg", false);
        eye_meshRender.material = eyes[0]; //0 == open eye 
        ps_sweating.Stop(); //stop sweating

        //reset
        curr_secBefore = 0;
        curr_secAfter = 0;
        curr_PetState = Pet_States.stage1;
    }



    //while depressed we can't do anything
    private void DepressedMode()
    {
        //have to escape depression to go back to normal states
        //for now just eat 10 food pellets 


        //if wer not targeting food (ie:current target food is null) 
        //          : target a food
        //          : and just idle this frame
        if (foodTarget == null)
        {
            NewFoodTarget_Tank();
            IdleMode();
        }
        else
        {
            //else
            //follow food
            //head towards target 
            UpdatePosition(foodTarget.transform.position, hungry_velocity);
        }

    }

    private void NewFoodTarget_Tank(){

        //new target
        NewTargetVariables();

        //find food to followe 
        var closestDis = float.PositiveInfinity;
        var allFoods = Controller_Food.instance.GetAllFood();
        if(allFoods.Count == 0){return;}

        //for all food objs in scene, get the closest
        var tempTarget = allFoods[0];
        foreach (GameObject food in allFoods){

            var newDis = (transform.position - food.transform.position).sqrMagnitude;

            if(newDis < closestDis){

                closestDis = newDis;
                tempTarget = food;  
            }
        }
        //
        foodTarget = tempTarget;
        
        //once the fish or the trash can gets to the food, the food destroysSelf(), and foodtarget = null again
    }



    //public method that is used by the collider class mostly, 
    //everytime mary eats food while depressed we want to increment food eaten
    public void EatedFood()
    {
        foodsEaten += 1;
        if (foodsEaten >= FoodAteThreshold)
        {
            //reset mary: only need to set to stage 1, from their she should be able to sort what stage she's in
            curr_PetState = Pet_States.stage1;
            foodsEaten = 0;
        }
    }


    //start of enemy wave event
    public override void Event_Init(Event_Type type, GameObject obj)
    {
        if (type != Event_Type.guppyDead) { return; }

        guppysDeathToll += 1;

        if (guppysDeathToll >= DeadGuppyThreshold)
        {
            //we enter depression
            curr_PetState = Pet_States.depressed;
            //reset
            guppysDeathToll = 0;
        }
    }

    //end of enemy wave event
    public override void Event_EndIt(Event_Type type)
    {

    }

}
