using System.Collections;
using UnityEngine;




//this scirpt is the mouth of _ enemy fish
//this script will be attached to its own gameobject that has a collider
//when ever this scripts gameobject is active, we can keep attacking
//once we have a collision we turn ourselves off and send a message to body/sm script saying we made contact
// this can lead to other states (ie idle mode)

public class _Mouth : MonoBehaviour
{

    [SerializeField] ParticleSystem bite_particle;

    private int attackPower;

    public void SetAttackPow(int power){
        attackPower = power;
    }

    private void OnTriggerStay2D(Collider2D other) {
        
        if(other.gameObject.CompareTag("Fish") || other.gameObject.CompareTag("Pet")){
            
            //bite
            //Debug.Log(gameObject.ToString() + "Bite");
            //get this gameobject's stats script and deal damage
            other.gameObject.GetComponent<FishStats_ParentClass>().TakeDamage(attackPower);
            
            //make particle
            Instantiate(bite_particle, transform.position, Quaternion.identity);
 
            //now send message


        }
    }

}
