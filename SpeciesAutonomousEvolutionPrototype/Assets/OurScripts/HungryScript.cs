using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungryScript : MonoBehaviour {
    private int frames = 0;
    Text hungryText;
    Text lifeText;
    SpeciesAttributes attributes;
    int character;

    void Start ()
    {
        attributes = GetComponent<SpeciesAttributes>();
        hungryText = GameObject.Find("HungryText").GetComponent<Text>();
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        character = int.Parse(gameObject.name);
    }
	
	void Update () {
        frames++;
        if(frames % 30 == 0)
        {
            frames = 0;
            if (attributes.hungry > 0)
            {
                attributes.hungry--;
            }
            else if (attributes.life > 0)
            {
                attributes.life--;
            }
            if (character == PlayerInfo.selectedCreature)
            {
                hungryText.text = attributes.hungry.ToString();
                lifeText.text = attributes.life.ToString();
            }
        }
	}
}
