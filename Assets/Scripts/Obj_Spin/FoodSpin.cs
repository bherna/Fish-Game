using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpin : MonoBehaviour
{
    [SerializeField] int speed = 0;
    [SerializeField] GameObject foodSprite;

    private void Update() {
        foodSprite.transform.Rotate(Time.deltaTime * speed, Time.deltaTime * speed, Time.deltaTime * speed, Space.Self);
    }
}
