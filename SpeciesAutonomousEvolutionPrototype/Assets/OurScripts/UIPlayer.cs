using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour {
    void Update () {
        gameObject.transform.position = new Vector3(150, 25, 0);
    }
}
