using UnityEngine;
using UnityEngine.EventSystems;

public class EventClick_test : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler

{
    
    private MaterialApplier materialApplier;

    private void Awake(){

        materialApplier = GetComponent<MaterialApplier>();
    }

    public void OnPointerDown(PointerEventData eventData){

        materialApplier.ApplyOther();
    }

    public void OnPointerUp(PointerEventData eventData){

        materialApplier.ApplyOriginal();
    }

    //down and release mouse click
    public void OnPointerClick(PointerEventData eventData){

        //emp
    }

    public void OnPointerEnter(PointerEventData eventData){

        //emp
    }
    public void OnPointerExit(PointerEventData eventData){

        //emp
    }



}
