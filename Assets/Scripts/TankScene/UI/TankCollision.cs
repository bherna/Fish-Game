using UnityEngine;

public class TankCollision : MonoBehaviour
{

    //reference to self (global)
    public static TankCollision instance {get; private set; }

    //reference to tank area collision box
    [SerializeField] BoxCollider2D swimRange;

    //reference to spawn collision box
    [SerializeField] BoxCollider2D spawnRange;

    //reference to boundry tag collision box
    [SerializeField] BoxCollider2D boundry;

    //trash can dimension for gizmo only
    [SerializeField] BoxCollider2D trash;

    //spawn range
    float spawn_xLower;
    float spawn_xUpper;
    float spawn_yLower;
    float spawn_yUpper;

    //swim range
    float swim_xLower;
    float swim_xUpper;
    float swim_yLower;
    float swim_yUpper;


    //boundry range
    float boundry_xLower;
    float boundry_xUpper;
    float boundry_yLower;
    float boundry_yUpper;

    //trashcan range
    float trash_xLower;
    float trash_xUpper;
    float trash_yLower;
    float trash_yUpper;


    private void Awake()
    {

        //delete duplicate of this instance
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }


        GetSwim_D();
        GetSpawn_D();
        GetBoundry_D();
        GetTrash_D();
    }


    //these methods are used to get tank collider dimensions, 

    private void GetSwim_D(){

        var w = swimRange.size.x;
        var h = swimRange.size.y;

        var tank_pos = swimRange.offset;

        swim_xLower = tank_pos.x - w/2;
        swim_xUpper = tank_pos.x + w/2;

        swim_yLower = tank_pos.y - h/2;
        swim_yUpper = tank_pos.y + h/2;

    }

    private void GetSpawn_D(){

        var w = spawnRange.size.x;
        var h = spawnRange.size.y;

        var tank_pos = spawnRange.offset;

        spawn_xLower = tank_pos.x - w/2;
        spawn_xUpper = tank_pos.x + w/2;

        spawn_yLower = tank_pos.y - h/2;
        spawn_yUpper = tank_pos.y + h/2;

    }

    private void GetBoundry_D(){

        var w = boundry.size.x;
        var h = boundry.size.y;

        var tank_pos = boundry.offset;

        boundry_xLower = tank_pos.x - w/2;
        boundry_xUpper = tank_pos.x + w/2;

        boundry_yLower = tank_pos.y - h/2;
        boundry_yUpper = tank_pos.y + h/2;

    }


    private void GetTrash_D() {
        
        var w = trash.size.x;
        var h = trash.size.y;

        var trash_pos = trash.offset;

        trash_xLower = trash_pos.x - w/2;
        trash_xUpper = trash_pos.x + w/2;

        trash_yLower = trash_pos.y - h/2;
        trash_yUpper = trash_pos.y + h/2;
    }



    //these can be used, if we need to do some dimension size math

    public (float, float, float, float) GetTankSwimArea() {
        return (swim_xLower, swim_xUpper, swim_yLower, swim_yUpper);
    } 

    public (float, float, float, float) GetTankSpawnArea(){
        return (spawn_xLower, spawn_xUpper, spawn_yLower, spawn_yUpper);
    } 
    
    public (float, float, float, float) GetBoundryArea(){
        return (boundry_xLower ,boundry_xUpper, boundry_yLower, boundry_yUpper);
    }

    public (float, float, float, float) GetTrashArea(){
        return (trash_xLower ,trash_xUpper, trash_yLower, trash_yUpper);
    }




    //method used for displaying the tank colliders together at once in inspector + scene
    protected void OnDrawGizmosSelected() {
   
        //swim range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(swimRange.offset, swimRange.size);

        //spawn range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(spawnRange.offset, spawnRange.size);

        //boundry
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boundry.offset, boundry.size);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(trash.offset, trash.size);

    }
}
