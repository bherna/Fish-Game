using System;
using Steamworks;
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

    private bool keepCountingBefore = true;
    private bool keepCountingAfter = true;
    



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
    }

    // Update is called once per frame
    private new void Update()
    {
        base.Update(); //incase

        IdleMode(); //movement
        

        //keep adding untill mary becomes pregnant
        if(keepCountingBefore)
        {
            curr_secBefore += Time.deltaTime;

            if(curr_secBefore >= max_secBefore){
                //stop counting
                keepCountingBefore = false;
                //mary is now pregnant
                animator.SetBool("isPreg", true);
                eye_meshRender.material = eyes[1]; //1 == closed eye 'blink'
                ps_sweating.Play(); //play sweating particles

            }
            return;
        }


        //same thing but now mary is pregnant
        if(keepCountingAfter)
        {
            curr_secAfter += Time.deltaTime;

            if(curr_secAfter >= max_secAfter){
                //stop counting
                keepCountingAfter = false;
            }
            return;
        }


        //if we are in enemy wave, return, since we don't want to spawn yet
        if (Controller_Enemy.instance.currently_in_wave){return;} 
        
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
        keepCountingBefore = true;
        keepCountingAfter = true;
        
        
    }



    //start of enemy wave event
    public override void Event_Init(Event_Type type, GameObject obj)
    {
 
    }

    //end of enemy wave event
    public override void Event_EndIt(Event_Type type)
    {

    }

}
