using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGlobalAttributes : MonoBehaviour {
    public int species;
    public int creaturesCount = 0;

    void Start () {
        GameObject.Find("EnemiesCreatures/Enemies0").GetComponent<EnemiesGlobalAttributes>().species = (PlayerInfo.selectedSpecies + 1) % 4;
        GameObject.Find("EnemiesCreatures/Enemies1").GetComponent<EnemiesGlobalAttributes>().species = (PlayerInfo.selectedSpecies + 2) % 4;
        GameObject.Find("EnemiesCreatures/Enemies2").GetComponent<EnemiesGlobalAttributes>().species = (PlayerInfo.selectedSpecies + 3) % 4;
    }
}

