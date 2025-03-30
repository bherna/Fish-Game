
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class EggPopUp : MonoBehaviour
{
    private enum EggStates {Idle, MoveToCenter, Desatsurate, Pop}
    private EggStates currEggState = EggStates.Idle;


    // -------------------------------------------- used in editing position --------------------------------------------
    private const int timeToTake_Position = 3; //number of seconds it should take our egg to reach center of screen (useful if we have a tune to play)
    private float currTimeAT = 0; //in interpolation
    private Vector2 startPos; //in interpolation
    private Vector2 endPos; 

    // -------------------------------------------- used in deSaturate() --------------------------------------------
    private Image petEgg; // image - pet egg child 
    private Color eggAlpha; //used in pop
    private const int timeToTake_Desaturation = 4;

    // -------------------------------------------- used in Pop() --------------------------------------------
    private const int timeToTake_Pop = 1;
    private const float popHeight = 100;



    void Start()
    {

        //set pet egg sprite
        petEgg = transform.GetChild(1).gameObject.GetComponent<Image>();
        string filePath = string.Format("EggsSprites/{0}_Full", LocalLevelVariables.GetUnlockPet_Name());
        petEgg.sprite = Resources.Load<Sprite>(filePath);


        //used in pop
        eggAlpha = petEgg.color;
        
    }

    //set start position for our egg
    public void StartEndPoints(Vector2 startPos, Vector2 endPos){

        transform.position = startPos;
        this.startPos = startPos;
        this.endPos = endPos;

    }

    //lets pop up controller have contol of when to start moving the sprite.
    public void StartMoving(){
        currEggState = EggStates.MoveToCenter;
    }

    // Update is called once per frame
    void Update()
    {

        switch(currEggState){

            case EggStates.Idle:
                //do nothing
                break;

            case EggStates.MoveToCenter:
                MoveToCenter();
                break;
                
            case EggStates.Desatsurate:
                Desatsurate();
                break;

            case EggStates.Pop:
                Pop();
                break;

            default:
                Debug.Log("Not in any known egg state.");
                break;
        }
        
    }


    private void MoveToCenter(){
        //while we are farther than our desired closenes 
        //keep approaching
        if( currTimeAT <= timeToTake_Position){

            //lerp to center of screen
            float interpolationRatio = currTimeAT / timeToTake_Position;
            transform.position = Vector2.Lerp(startPos, endPos, interpolationRatio);

            currTimeAT += Time.deltaTime;

        }
        else{
            //we should be close enough
            //move our egg straight to vector zero
            transform.position = endPos;
            
            //and stop movement code
            currTimeAT = 0;
            currEggState = EggStates.Desatsurate;
        }
    }

    private void Desatsurate(){

        //slowly turn down saturation on egg image (make it look like its glowing)
        //we are just going to use alpha with a white egg bg to get this effect, i aint figuring out how to desaturate an image right now
        // (pretty much same code as movetocenter)
        if( currTimeAT <= timeToTake_Desaturation){

            float interpolationRatio = currTimeAT / timeToTake_Desaturation;
            eggAlpha.a = Mathf.Lerp(100, 0, interpolationRatio); 

            currTimeAT += Time.deltaTime;

        } 
        //once fully desaturated, instantiate new unlocked pet as animation infront of egg
        else{

            //update image to be new pet 
            string filePath = string.Format("EggsSprites/{0}_Full", LocalLevelVariables.GetUnlockPet_Name()); //update to where pet animatins will be
            petEgg.sprite = Resources.Load<Sprite>(filePath);
            
            //reset and move to next state
            currTimeAT = 0;
            currEggState = EggStates.Pop;
        }

    }


    private void Pop(){

        //now we are going to play a mini animation of the pet hatching from the egg, like pokemon (R)
        // (pretty much same code as movetocenter)
        if( currTimeAT <= timeToTake_Pop){

            //re-add alpha to pet image
            float interpolationRatio = currTimeAT / timeToTake_Pop;
            eggAlpha.a = Mathf.Lerp(0, 100, interpolationRatio); 

            //up down animation
            float offset = popHeight * Mathf.Sin(interpolationRatio * Mathf.PI);
            Vector2 vZero = endPos;
            vZero.y += offset;
            transform.GetChild(1).transform.position = vZero;
            
            //update
            currTimeAT += Time.deltaTime;
            
        } 
        //once we are done with that, we can finally call the continue button from objective controller
        else{
            //but dont forget to stop doing this code again
            currEggState = EggStates.Idle;

            //now button activate !
            Controller_Objective.instance.ActivatePostEndGameUI();
        }
        
        
    }



}
