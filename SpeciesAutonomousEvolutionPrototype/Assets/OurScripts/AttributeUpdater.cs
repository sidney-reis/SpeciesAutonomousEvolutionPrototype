using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeUpdater : MonoBehaviour {
    SpeciesAttributes attributes;
    int character;

    void Start () {
        attributes = GetComponent<SpeciesAttributes>();
        character = int.Parse(gameObject.name);
        InvokeRepeating("IncreaseLibido", 0, 1);
        InvokeRepeating("DecreaseHungry", 0, 1);
    }

    void IncreaseLibido()
    {
        Text libidoText = GameObject.Find("Canvas/AttributeBackground/LibidoText").GetComponent<Text>();
        if (attributes.libido < 200)
        {
            attributes.libido++;
            if (character == PlayerInfo.selectedCreature)
            {
                libidoText.text = attributes.libido.ToString();
            }
        }
    }

    void DecreaseHungry()
    {
        Text lifeText = GameObject.Find("LifeText").GetComponent<Text>();
        Text hungryText = GameObject.Find("HungryText").GetComponent<Text>();

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
