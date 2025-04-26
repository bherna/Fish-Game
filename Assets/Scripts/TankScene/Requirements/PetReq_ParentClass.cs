
using UnityEngine;

public class PetReq_ParentClass : MonoBehaviour
{

    //this class will have every requirement reference built into it, but will be empty
    //since this is going to be a virtual class

    //one of the pet name classes needs to be used in a real level


    //what every pet egg hatch requirement will need is a $/second 
    //  - this is just a simple income requirement, seems stupid but what ever
    //      this is just to get people to not rely on simple guppy builds. I want players to figure out new income methods that will be unlocked throughout their gameplay 
    protected int income =  0;   //this is the one that is activly changing evry second
    protected int income_req =  0; //this is the one we want to achive (shoulld be updated in child classes)


    public void SetIncome(int newIncome){
        income = newIncome;

        PostUpdates();
    }






    //used in stopping checks
    public bool toggle {get; protected set;}= false;
    public virtual void StartReqs(){
        toggle = true;
    }


    //singleton this class
    public static PetReq_ParentClass instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }




    // Start is called before the first frame update
    protected virtual void Start()
    {
        //display requirements in UI element (not really nessesary since the ui is inactive at this point)
        PostUpdates();
    }


    //used after updating any of our requirements, one to update the text in the ui, and another to check if completed
    //THIS CLASS SHOULD BE OVERRIDED, per pet class
    protected virtual void PostUpdates(){

        if(!toggle){return;}

        string ourTex = string.Format("Requirements:\nNULLLLLLLLLLLLLLLLLL");

        Controller_Requirements.instance.UpdateReqs(ourTex);
    }


    //used in only getting guppys of certian age group, 
    //param #1: what their new age is
    //param #1: if this is a decrement or increment 
    //Example,  a guppy become a teen, so age = 1, and val = 1, 
    //       if a guppy gets spawnd,    age = 0, and val = 1,
    //only time val is -1, is when they die, so
    //Example,  a guppy is a adult dies from starvation, age = 2 and val = -1
    //what this functino will do, will be determined by the pet egg
    public virtual void UpdateGuppyCounter_Age(int age, int val){}


    //event is used every time a guppy eats a pellet
    public virtual void GuppyAte(){}

    //call when max food in shop is bought
    public virtual void MaxFoodReached(){}

}
