using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapsManager : MonoBehaviour
{
    public static LapsManager instance { get { return _instance; } }
    private static LapsManager _instance;

    [Header("Checkpoints and Goals")]
    public GameObject aiGoal;
    public GameObject lapsGoal;
    public GameObject[] checkPoints;

    public int currentCP { get { return cpPassed; } set { cpPassed = value; } }
    public int totalCP { get { return checkPoints.Length; } set { totalCP = value; } }
    private int cpPassed = 0;

    [Header("Total Laps")]
    public int totalLaps = 3;
    private int startLaps = 1;
    public int currentLaps { get { return startLaps; } set { startLaps = value; } }

    [HideInInspector]
    public int aiCurrentGoal = 0;
    public int aiMaxGoal { get { return totalLaps; } set { totalLaps = value; } }

    [Header("Timer")]
    public Timer timer;

    private void Awake()
    {
        // Singleton for LapsManager
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = timer.GetComponent<Timer>();
        aiGoal.SetActive(true);
        lapsGoal.SetActive(true);
        foreach (GameObject cp in checkPoints)
        {
            cp.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLaps > totalLaps)
        {
            currentLaps = totalLaps;
            GameManager.instance.PlayerWin();
            aiGoal.SetActive(false);
        }

        if (aiCurrentGoal > aiMaxGoal)
        {
            lapsGoal.SetActive(false);
            GameManager.instance.PlayerLose();
            aiGoal.SetActive(false);
        }
    }

    public void ResetLaps()
    {
        cpPassed = 0;

        foreach (GameObject cp in checkPoints)
        {
            cp.SetActive(true);
        }
    }
}
