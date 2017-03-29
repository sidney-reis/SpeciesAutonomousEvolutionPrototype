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

    // Use this for initialization
    void Start ()
    {
        attributes = GetComponent<SpeciesAttributes>();
        hungryText = GameObject.Find("HungryText").GetComponent<Text>();
        lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        character = int.Parse(gameObject.name);
    }
	
	// Update is called once per frame
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
            if (character == SpeciesSellector.selectedCharacter)
            {
                hungryText.text = attributes.hungry.ToString();
                lifeText.text = attributes.life.ToString();
            }
        }
	}
}
