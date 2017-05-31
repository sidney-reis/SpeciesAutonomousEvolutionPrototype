using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {
    private GameObject menuBackground;
    private GameObject breedMenu;
    private GameObject combatMenu;

    void Start()
    {
        menuBackground = gameObject.transform.Find("MenuBackground").gameObject;
        breedMenu = gameObject.transform.Find("MenuBackground/BreedMenu").gameObject;
        combatMenu = gameObject.transform.Find("MenuBackground/CombatMenu").gameObject;
    }
    public void CloseCurrentMenu ()
    {
        breedMenu.SetActive(false);
        combatMenu.SetActive(false);
        menuBackground.SetActive(false);
    }
    public void OpenReproduceMenu ()
    {
        menuBackground.SetActive(true);
        breedMenu.SetActive(true);
    }
    public void OpenCombatMenu()
    {
        menuBackground.SetActive(true);
        combatMenu.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            CloseCurrentMenu();
        }
    }
}
