using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICounter : MonoBehaviour {
    Image creatureImage;
    Text counter;

    void Start () {
        counter = GameObject.Find("CreaturesCounter/Counter").GetComponent<Text>();
        creatureImage = GameObject.Find("CreaturesCounter/SpeciesIcon").GetComponent<Image>();
	}
	
	void Update () {
        creatureImage.sprite = Resources.Load<Sprite>(PlayerInfo.selectedSpecies + "-icon");
        gameObject.transform.position = new Vector3(60, 38, 0);
        string totalCreatures = PlayerInfo.playerCreaturesCount.ToString();
        string currentCreature = (PlayerInfo.selectedCreature + 1).ToString();

        counter.text = currentCreature + "/" + totalCreatures;
    }
}
