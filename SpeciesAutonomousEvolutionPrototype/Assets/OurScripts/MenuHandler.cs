using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {
    private GameObject menuBackground;
    private GameObject breedMenu;
    private GameObject combatMenu;
    private Text errorBreed;
    private Text errorCombat;

    void Start()
    {
        menuBackground = gameObject.transform.Find("MenuBackground").gameObject;
        breedMenu = gameObject.transform.Find("MenuBackground/BreedMenu").gameObject;
        combatMenu = gameObject.transform.Find("MenuBackground/CombatMenu").gameObject;
        errorBreed = gameObject.transform.Find("MenuBackground/BreedMenu/ErrorText").GetComponent<Text>();
        errorCombat = gameObject.transform.Find("MenuBackground/CombatMenu/ErrorText").GetComponent<Text>();
    }
    public void CloseCurrentMenu ()
    {
        breedMenu.SetActive(false);
        combatMenu.SetActive(false);
        menuBackground.SetActive(false);
        errorBreed.text = "";
        errorCombat.text = "";
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
