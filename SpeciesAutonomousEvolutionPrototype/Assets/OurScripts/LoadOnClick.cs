using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadOnClick: MonoBehaviour {
    public void LoadScene(int species)
    {
        SceneManager.LoadScene(1);
        PlayerInfo.selectedSpecies = species;
        Debug.Log("species: " + PlayerInfo.selectedSpecies);
    }
}
