using UnityEngine;
using System.Collections;

public class SpeciesSellector : MonoBehaviour {
    void Start () {
	}
	
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "ControllableSpecies")
                {
                    Debug.Log("Selecting Character");
                    PlayerInfo.selectedCreature = int.Parse(hitInfo.transform.gameObject.name);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int currentCreature = PlayerInfo.selectedCreature + 1;
            while(GameObject.Find("PlayerCreatures/"+currentCreature.ToString()) == null)
            {
                if (currentCreature == PlayerInfo.playerCreaturesCount)
                {
                    currentCreature = 0;
                }
                else
                {
                    currentCreature++;
                }
            }
            PlayerInfo.selectedCreature = currentCreature;

        }
    }
}
