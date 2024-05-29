using UnityEngine;

public class FoodValue : MonoBehaviour
{
    [SerializeField]
    private int foodValue;

    public int GetFoodValue(){
        return foodValue;
    }
}
