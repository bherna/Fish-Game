using UnityEngine;

//feed = just regular food to fill stomach
//burger = for burger type from dr. crabs
public enum FoodTypes {feed, burger}

public class Food_Parent : MonoBehaviour
{

    //feed = how much this food fills the stomach
    //burger = not important (maybe future updated to include multlpe stages skipped)
    [SerializeField] private int foodValue;  

    [SerializeField] public FoodTypes foodType;


    public int GetFoodValue(){
        return foodValue;
    }



}
