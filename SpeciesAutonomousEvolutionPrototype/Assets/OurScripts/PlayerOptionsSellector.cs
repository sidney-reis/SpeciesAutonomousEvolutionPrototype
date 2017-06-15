using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOptionsSellector : MonoBehaviour {
    private GameObject menuCanvas;
    public RawImage creatureImage;

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
                        creatureImage.texture = Resources.Load<Texture>(PlayerInfo.selectedSpecies.ToString()+"-bubble");
                        menuCanvas.GetComponent<MenuHandler>().CloseCurrentMenu();
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
                    string parentName = hitInfo.transform.parent.gameObject.transform.name;
                    PlayerInfo.selectedMenuEnemySpecies = Int32.Parse(parentName.Substring(parentName.Length - 1, 1)); 
                    
                    int speciesNumber = hitInfo.transform.parent.gameObject.GetComponent<EnemiesGlobalAttributes>().species;

                    if (PlayerInfo.selectedCreature >= 0)
                    {
                        creatureImage.texture = Resources.Load<Texture>(speciesNumber.ToString() + "-bubble");
                        menuCanvas.GetComponent<MenuHandler>().CloseCurrentMenu();
                        menuCanvas.GetComponent<MenuHandler>().OpenCombatMenu();
                    }
                    else
                    {
                        menuCanvas.GetComponent<MenuHandler>().CloseCurrentMenu();
                    }
                }
            }
        }
    }
}
