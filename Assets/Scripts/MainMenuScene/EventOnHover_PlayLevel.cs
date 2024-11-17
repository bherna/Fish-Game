using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventOnHover_PlayLevel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData){
        
        ToolTip.ShowToolTip("Play Level!");
    }

    public void OnPointerExit(PointerEventData eventData){
        ToolTip.HideToolTip();
    }
}
