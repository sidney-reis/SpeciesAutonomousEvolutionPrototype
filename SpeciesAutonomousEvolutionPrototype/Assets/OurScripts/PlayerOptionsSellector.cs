using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptionsSellector : MonoBehaviour {
    private GameObject menuCanvas;

    void Start ()
    {
        menuCanvas = GameObject.Find("MenuCanvas");
    }

    void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                if (hitInfo.transform.gameObject.tag == "ControllableSpecies")
                {
                    PlayerInfo.selectedOption = 0;
                    PlayerInfo.selectedMenuCreature = int.Parse(hitInfo.transform.gameObject.name);
                    if ((PlayerInfo.selectedCreature >= 0) && (PlayerInfo.selectedCreature != PlayerInfo.selectedMenuCreature))
                    {
                        menuCanvas.GetComponent<MenuHandler>().OpenReproduceMenu();
                    }
                    else
                    {
                        menuCanvas.GetComponent<MenuHandler>().CloseCurrentMenu();
                    }
                }
                else if (hitInfo.transform.gameObject.tag == "EnemySpecies")
                {
                    PlayerInfo.selectedOption = 1;
                    PlayerInfo.selectedMenuEnemy = int.Parse(hitInfo.transform.gameObject.name);
                }
            }
        }
    }
}
