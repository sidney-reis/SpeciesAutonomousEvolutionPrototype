using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesAttributeUpdater : MonoBehaviour {
    EnemiesAttributes attributes;

    void Start()
    {
        attributes = GetComponent<EnemiesAttributes>();
        InvokeRepeating("IncreaseLibido", 0, 1);
        InvokeRepeating("DecreaseHungry", 0, 1);
    }

    void IncreaseLibido()
    {
        if (attributes.libido < 200)
        {
            attributes.libido++;
        }
    }

    void DecreaseHungry()
    {
        if (attributes.hungry > 0)
        {
            attributes.hungry--;
        }
        else if (attributes.life > 0)
        {
            attributes.life--;
        }
    }
}
