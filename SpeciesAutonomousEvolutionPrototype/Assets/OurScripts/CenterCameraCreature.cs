using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCameraCreature : MonoBehaviour {
    void Update () {
		if(PlayerInfo.selectedCreature != -1)
        {
            GameObject currentCreature = GameObject.Find("PlayerCreatures/" + PlayerInfo.selectedCreature.ToString());
            if (currentCreature)
            {
                Vector3 newCameraPosition = new Vector3(currentCreature.transform.position.x, gameObject.transform.position.y, currentCreature.transform.position.z - 60);
                gameObject.transform.position = newCameraPosition;
            }
            
        }

	}
}
