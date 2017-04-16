using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    GameObject menuBackground;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(150, 75, 0);
    }
}
