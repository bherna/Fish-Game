

public class PetReq_Mary : PetReq_ParentClass
{

    //fyi mary will be the first pet with enemy waves


    //keeps track off baby guppys
    private int babies = 0;

    //how many babies do we want in tank as req
    private const int babies_req = 7;





    public override void StartReqs()
    {
        base.StartReqs();

        //update parent variables
        income_req = 120;

        //when we start, we need to get all guppys at age 0
        babies = Controller_Fish.instance.HowManyGuppysAtAge_(0);

        //update requiremtns board + checker (last)
        PostUpdates();

        //start the income caller, since we want to keep track of that stuff
        StartCoroutine(Controller_Wallet.instance.CalculateIncome());

    }



    //everytime a new update to our reqs changes, we update the visuals, and we chceck to see if we completed reqs
    protected override void PostUpdates(){

        if(!toggle){return;}

        string ourTex = string.Format("Requirements:\n~~~~~~~~~~~~~~~~\nBaby Guppys: {0} / {1}\n\nIncome: ${2} / ${3}", babies, babies_req, income, income_req);
        Controller_Requirements.instance.UpdateReqs(ourTex);


        //did we complete our reqs
        if(babies >= babies_req && income >= income_req){
            
            //we done, and we can stop
            toggle = false;

            //run post game thingy
            Controller_Objective.instance.LevelComplete();

        }

    }


    //mary's egg will need to know how many guppy babys are currently in the tank
    //the only time a guppy can become age 0, is when they are spawned (so controller_fish class)
    //the only time that a guppy can leave age 0, is when they get a birthday call (guppy_stats class)
    //or when they die from starvation/enemy (guppy_stats)
    public override void UpdateGuppyCounter_Age(int age, int val){

        //if age is baby or teen, we do something
        switch(age){

            case 0:
                //since this is just baby age we increment by val
                babies += val;
                break;


            case 1:
                //for teen we have a specific case
                //if our guppy had a birthday, they left baby stage, so we have to decrement
                if(val == 1){
                    babies -= 1;
                }
                break;


            //no default here
        }

        
        //update requiremtns board + checker
        PostUpdates();

                
    }




}
