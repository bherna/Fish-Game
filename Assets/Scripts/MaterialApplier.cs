using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialApplier : MonoBehaviour
{
    
    //this script is attached to the sprite render object
    [SerializeField] SpriteRenderer sprite_ren;

    [SerializeField] Sprite original_s;
    [SerializeField] Sprite other_s;

    


    public void ApplyOriginal(){
        sprite_ren.sprite = original_s;
    }

    public void ApplyOther(){
        sprite_ren.sprite = other_s;
    }

}
