
using System.Collections;
using UnityEngine;

public class Drop_Money : Drop_Parent
{
    [SerializeField] protected int moneyValue = 0;
    [SerializeField] protected AudioClip collectCoinSoundClip;



    public void UpdateCoinVal(int newMonVal){
        moneyValue = newMonVal;
    }
    


    public override void OnMouseDown()
    {

        Collect();
 
    }

    protected virtual void Collect()
    {
        //add coin
        Controller_Wallet.instance.AddMoney(moneyValue);

        //playsound
        //add a bit of randomness to the pitch to add variance
        var pitch = Random.Range(0.85f, 1.15f);
        AudioManager.instance.PlaySoundFXClip(collectCoinSoundClip, transform, 1f, pitch);

        //instantiate text pop up
        Controller_PopUp.instance.CreateTextPopUp(string.Format("+ {0}", moneyValue));
        //destroy
        Destroy(gameObject);
    }




    public override void OnTrashDrop()
    {
        base.OnTrashDrop();
        //event send to pets
        Controller_Pets.instance.Annoucement_Init(Event_Type.coin, gameObject);
    }


}
