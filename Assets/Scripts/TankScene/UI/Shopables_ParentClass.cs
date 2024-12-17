
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Shopables_ParentClass : MonoBehaviour
{

    //the currently displayed sprite for this button
    [SerializeField] protected Image currSprite;

    
    //event listeners
    //this means that every shopable should have the onpurhcase func
    void OnEnable()
    {
        //Register Button Events
        GetComponent<Button>().onClick.AddListener(() => OnPurchase());
    }

    void OnDisable()
    {
        //Un-Register Button Events
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
    


    ///this should be updated in each shopable class
    public abstract void OnPurchase(); //do nothing here
    public abstract void OnPointerEnter(PointerEventData eventData);

    //we dont need to update this one, else we can
    public virtual void OnPointerExit(PointerEventData eventData){
        ToolTip.HideToolTip();
    }
}
