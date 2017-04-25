using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTime : MonoBehaviour {
    public static long totalSeconds = 0;
    public static long currentSeconds = 0;

    void Start () {
        InvokeRepeating("Timer", 0, 1);
    }

    void Timer()
    {
        totalSeconds++;
        currentSeconds++;
    }

    public static void triggerBreed()
    {
        currentSeconds = 0;
    }
}
