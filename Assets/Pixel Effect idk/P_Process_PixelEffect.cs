using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class P_Process_PixelEffect : MonoBehaviour
{
    public Material effect;


    //src = camera
    //dest = render final with shader applied
    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        
        //copy src to dest
        Graphics.Blit(src, dest, effect);
    }
}
