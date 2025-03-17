using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EggColliders
{
    
    public class ColliderDem
    {
        public Vector2 offset;
        public Vector2 size;
        public CapsuleDirection2D orientation;

        public ColliderDem(Vector2 offset, Vector2 size, CapsuleDirection2D orientation){
            this.offset = offset;
            this.size = size;
            this.orientation = orientation;
        }

    }


    //crack type one 
    private static ColliderDem Egg_0 = new ColliderDem(new Vector2(0f, -1.08f), new Vector2(3.7f, 3.97f), CapsuleDirection2D.Horizontal);
    private static ColliderDem Egg_0_1 = new ColliderDem(new Vector2(0f, -0.57f), new Vector2(3.25f, 3.84f), CapsuleDirection2D.Horizontal);
    private static ColliderDem Egg_0_1_2 = new ColliderDem(new Vector2(0f, 0f), new Vector2(3.62f, 5.06f), CapsuleDirection2D.Vertical);
    private static ColliderDem Egg_1 = new ColliderDem(new Vector2(0f, 0.16f), new Vector2(4.23f, 2.7f), CapsuleDirection2D.Horizontal);
    private static ColliderDem Egg_1_2 = new ColliderDem(new Vector2(0f, 0.73f), new Vector2(4.04f, 3.01f), CapsuleDirection2D.Horizontal);
    private static ColliderDem Egg_2 = new ColliderDem(new Vector2(0f, 1.64f), new Vector2(3.33f, 1.44f), CapsuleDirection2D.Horizontal);

    public static Dictionary<string, ColliderDem> crack1 = new Dictionary<string, ColliderDem>{
        {"Egg_0", Egg_0},
        {"Egg_0_1", Egg_0_1},
        {"Egg_0_1_2", Egg_0_1_2},
        {"Egg_1", Egg_1},
        {"Egg_1_2", Egg_1_2},
        {"Egg_2", Egg_2},
        
    };
    

    //crack type two.


    //...
}
