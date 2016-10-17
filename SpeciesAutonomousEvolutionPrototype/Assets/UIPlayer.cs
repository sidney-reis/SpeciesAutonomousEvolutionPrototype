using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour {
    GameObject attributeBackground;
    
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        attributeBackground = GameObject.Find("AttributeBackground");
        attributeBackground.transform.position = new Vector3(150, 25, 0);

    }
}
