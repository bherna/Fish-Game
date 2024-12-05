#if UNITY_EDITOR
using UnityEngine;


//first class holds all methods that the tester can use
//we only use this class as a reference, dont use start or update methods here

public class TestFunctions : MonoBehaviour 
{

    //simple function to update our timescale in the game, 
    //useful to test anything relative to time 
    public int timeSpeed = 1;
    public void UpdateTimeSpeed(){
        Time.timeScale = timeSpeed;
    }

    //straight up just give tester money
    //useful in testing if shop items are purchasable / have correct functionality
    public int money = 100;
    public void GiveMoney(){
        Controller_Wallet.instance.AddMoney(money);
    }

    //function to spawn a pet into the level
    //useful in testing a certain pet, even if its not unlocked to tester in game
    public void SpawnPet(PetNames petName){
        Controller_Pets.instance.Test_SpawnPet(petName);
    }



    //function to test the combo mech
    public int comboAmount = 1;
    public void AddToCombo(){
        UI_Combo.instance.AddToCombo(comboAmount);
    }
    
}

#endif