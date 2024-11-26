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

        //nmake sure that PetsAccess has current_Pets_slotted not null
        //else don't spawn any pets
        if(PetsAccess.current_pets_slotted == null){return;}

        //random position setup (get tank dimensions)
        var spawnArea = TankCollision.instance.GetTankSpawnArea();

        //spawn pets into tank
        foreach (PetNames pet in PetsAccess.current_pets_slotted){

            var spawnPoint = new Vector3(
                Random.Range(spawnArea.Item1, spawnArea.Item2),
                Random.Range(spawnArea.Item3, spawnArea.Item4),
                transform.position.z
            );

            //spawn new pet on screen and add to list
            if(pet != PetNames.Missing){
                pet_list.Add(Instantiate(Resources.Load("Pets/" + "Pet_"+pet.ToString()) as GameObject, spawnPoint, quaternion.identity));
            }
        }
    }

    


    //USED FOR TESTING ONLY
    //dont spawn pets normally thorugh here
    public void Test_SpawnPet(PetNames petNames){

        //pet_list.Add(Instantiate(Resources.Load("Pets/" + "Pet_SchoolTeacher") as GameObject, Vector2.zero, quaternion.identity));
        //pet_list.Add(Instantiate(Resources.Load("Pets/" + "Pet_DrCrabs") as GameObject, Vector2.zero, quaternion.identity));
        //pet_list.Add(Instantiate(Resources.Load("Pets/" + "Pet_MaryFertile") as GameObject, Vector2.zero, quaternion.identity));
        //pet_list.Add(Instantiate(Resources.Load("Pets/" + "Pet_Khalid") as GameObject, Vector2.zero, quaternion.identity));
        //pet_list.Add(Instantiate(Resources.Load("Pets/" + "Pet_Salt") as GameObject, Vector2.zero, quaternion.identity));

        pet_list.Add(Instantiate(Resources.Load("Pets/Pet_"+petNames.ToString()) as GameObject, Vector2.zero, quaternion.identity));
    }



    //function call for controller enemy
    //tells controller pet that enemy waves have ended or started
    //if we have any pets in tank that run ability of that, tell those pets
    public void Annoucement_Init(Event_Type type, GameObject obj){
        
        foreach(GameObject pet in pet_list){
            pet.GetComponent<Pet_ParentClass>().Event_Init(type, obj);
        }
    }

    public void Annoucement_EndIt(Event_Type type){
        
        foreach(GameObject pet in pet_list){
            pet.GetComponent<Pet_ParentClass>().Event_EndIt(type);
        }
    }


    
}
