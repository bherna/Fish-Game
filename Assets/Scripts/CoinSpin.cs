using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpin : MonoBehaviour
{
    [SerializeField] int speed = 0;
    [SerializeField] GameObject coinSprite;

    // Update is called once per frame
    void Update()
    {
        coinSprite.transform.Rotate(Time.deltaTime * speed, 0, 0, Space.Self);
    }
}
