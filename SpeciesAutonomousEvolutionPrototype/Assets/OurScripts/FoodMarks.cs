using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMarks : MonoBehaviour {
    public Hashtable speciesHunting = new Hashtable();

    void Start ()
    {
        speciesHunting.Add(0, -1);
        speciesHunting.Add(1, -1);
        speciesHunting.Add(2, -1);
        speciesHunting.Add(3, -1);
    }
}
