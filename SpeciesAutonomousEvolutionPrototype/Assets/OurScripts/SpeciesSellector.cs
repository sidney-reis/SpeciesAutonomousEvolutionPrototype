using UnityEngine;
using System.Collections;

public class SpeciesSellector : MonoBehaviour {
	void Update () {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "ControllableSpecies")
                {
                    //Debug.Log("Selecting Character");
                    PlayerInfo.selectedCreature = int.Parse(hitInfo.transform.gameObject.name);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject currentCreature = GameObject.Find("PlayerCreatures/" +PlayerInfo.selectedCreature);
            if (currentCreature == null)
            {
                PlayerInfo.selectedCreature = int.Parse(GameObject.Find("PlayerCreatures").transform.GetChild(0).name);
            }
            else if(currentCreature.transform.GetSiblingIndex() == currentCreature.transform.parent.childCount-1)
            {
                PlayerInfo.selectedCreature = int.Parse(GameObject.Find("PlayerCreatures").transform.GetChild(0).name);
            }
            else
            {
                Transform nextChild = currentCreature.transform.parent.GetChild(currentCreature.transform.GetSiblingIndex() + 1);
                PlayerInfo.selectedCreature = int.Parse(nextChild.name);
            }
            
        }
    }
}
