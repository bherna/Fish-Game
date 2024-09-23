using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller_Pets : MonoBehaviour
{
    //all this class should do for now,
    //spawn the (upto) 3 pets that the player has choosen in the menu screen


    // Start is called before the first frame update
    void Start()
    {
        //nmake sure that PetsAccess has current_Pets_slotted not null
        if(PetsAccess.current_pets_slotted == null){return;}

        //random position setup
        var spawnArea = TankCollision.instance.GetTankSpawnArea();

        //spawn pets into tank
        foreach (string pet in PetsAccess.current_pets_slotted){

            var spawnPoint = new Vector3(
                Random.Range(spawnArea.Item1, spawnArea.Item2),
                Random.Range(spawnArea.Item3, spawnArea.Item4),
                transform.position.z
            );

            Instantiate(Resources.Load("Pets/" + pet) as GameObject, spawnPoint, quaternion.identity);
        }
    }

    
}
