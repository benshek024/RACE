using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float bestLapTime { get; private set; } = Mathf.Infinity;
    public float lastLapTime { get; private set; } = 0;
    public float currentLapTime { get; private set; } = 0;
    private float lapTimer;

    private void Start()
    {
        StartLap();
    }

    // Update is called once per frame
    void Update()
    {
        currentLapTime = lapTimer > 0 ? Time.time - lapTimer : 0;
        //UpdateTimer();
    }

    public void StartLap()
    {
        lapTimer = Time.time;
    }

    public void EndLap()
    {
        lastLapTime = currentLapTime - lastLapTime;
        bestLapTime = Mathf.Min(lastLapTime, bestLapTime);
        Debug.Log("End Lap - LapTime was " + lastLapTime + "seconds.");
    }
}
