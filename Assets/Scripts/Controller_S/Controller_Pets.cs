using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller_Pets : MonoBehaviour
{
    //all this class should do for now,
    //spawn the (upto) 3 pets that the player has choosen in the menu screen
    [SerializeField] List<GameObject> pet_list;




    //singleton this class
    public static Controller_Pets instance {get; private set; }
    void Awake (){

        //delete duplicate of this instance

        if (instance != null && instance != this){
            Destroy(this);
        }
        else{
            instance = this;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        //create empty pet list
        pet_list = new List<GameObject>();

        //--------------------------------------------DELETE THIS LATER
        SpawnPets();

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

            //spawn new pet on screen and add to list
            pet_list.Add(Instantiate(Resources.Load("Pets/" + pet) as GameObject, spawnPoint, quaternion.identity));
        }
    }

    

    //override spawn pets
    //USED FOR TESTING ONLY
    private void SpawnPets(){

        Debug.Log("SPAWNED PETS ILLEGALLY");
        pet_list.Add(Instantiate(Resources.Load("Pets/" + "Test_Pet") as GameObject, Vector2.zero, quaternion.identity));
    }



    //function call for controller enemy
    //tells controller pet that enemy waves have ended
    //if we have any pets in tank that run ability of that, tell those pets
    public void Annoucement_NoMoreEnemies(){

    }
}
