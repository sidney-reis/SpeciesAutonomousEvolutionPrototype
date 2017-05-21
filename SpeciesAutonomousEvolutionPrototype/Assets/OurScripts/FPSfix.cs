using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSfix : MonoBehaviour {
    void Awake()
    {
        Application.targetFrameRate = 24;
    }
}
