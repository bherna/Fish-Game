using UnityEngine;



public class Fish_Age : MonoBehaviour
{

    //fish stage
    public int current_age_stage {get; private set; } = 0;


    [SerializeField] float amount_food_ate = 0;
    [SerializeField] int food_until_next_stage = 3;
    [SerializeField] Transform sprite_render;

    //bool if we should keep age-ing
    private bool updateAge = true;

    [SerializeField] private float fish_size_current = 1;
    [SerializeField] private float fish_size_scale = 0.5f;




    private void Start() {

        sprite_render.transform.localScale = new Vector3(fish_size_current, fish_size_current, fish_size_current);

    }
 

    public void Ate(){

        if(updateAge){

            //update age of fish
            amount_food_ate += 1;

            if(amount_food_ate >= food_until_next_stage){
                UpdateFishStage();
            }
        }
    }

    private void UpdateFishStage(){

        //if current age is not final stage
        if(current_age_stage < Controller_Fish.instance.GetFishStages().Count-1){
            
            //update 
            current_age_stage += 1;
            fish_size_current += fish_size_scale;
            sprite_render.transform.localScale = new Vector3(fish_size_current, fish_size_current, fish_size_current);

            //reset
            amount_food_ate = 0;
            
        }
        else{
            //we are done aging
            updateAge = false;
        }
    }
}
