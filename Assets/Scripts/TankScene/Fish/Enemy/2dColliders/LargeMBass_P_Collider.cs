using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeMBass_P_Collider : MonoBehaviour
{
    

    private LargeMBass_SM largeMBass_SM;

    // Start is called before the first frame update
    protected void Start()
    {

        largeMBass_SM = GetComponent<LargeMBass_SM>();

    }

    //this isn't the normal onplayerclick method, 
    //this is our own version to avoid calling it here (should be called from {enemyname}_collider)
    public void OnMouseDown()
    {
        largeMBass_SM.On_PlayerClick();
    }
}
