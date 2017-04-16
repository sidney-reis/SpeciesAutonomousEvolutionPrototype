using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LibidoScript : MonoBehaviour {
    Text libidoText;
    SpeciesAttributes attributes;
    int character;
    private float generateLibidoInSeconds = 1;

    void Start()
    {
        attributes = GetComponent<SpeciesAttributes>();
        libidoText = GameObject.Find("Canvas/AttributeBackground/LibidoText").GetComponent<Text>();
        character = int.Parse(gameObject.name);
        InvokeRepeating("IncreaseLibido", 0, generateLibidoInSeconds);
    }

    void IncreaseLibido()
    {
        if (attributes.libido < 150)
        {
            attributes.libido++;
            if (character == PlayerInfo.selectedCreature)
            {
                libidoText.text = attributes.libido.ToString();
            }
        }
    }
}
