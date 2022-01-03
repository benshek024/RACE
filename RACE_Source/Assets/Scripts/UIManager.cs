using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get { return _instance; } }
    private static UIManager _instance;

    public bool isPaused = false;

    [Header("Text")]
    public Text lapsText;
    public Text timerText;
    public Text lastText;
    public Text bestText;

    private int currentLap = 1;
    private float currentLapTime;
    private float lastLapTime;
    private float bestLapTime;

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject confirmPanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject startPanel;

    private Animator winPanelAnim;
    private Animator losePanelAnim;

    [Header("Timer")]
    public Timer timer;

    [Header("UI's SFX")]
    public AudioSource audioSource;
    public AudioClip pauseClip;

    private void Awake()
    {
        // Singleton for UIManager
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
        winPanelAnim = winPanel.GetComponent<Animator>();
        losePanelAnim = losePanel.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        startPanel.SetActive(true);
        confirmPanel.SetActive(false);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        timer = timer.GetComponent<Timer>();
        lapsText = lapsText.GetComponent<Text>();
        lapsText.text = "L A P : " + LapsManager.instance.currentLaps + " / " + LapsManager.instance.totalLaps;

        lastText.text = "LAST : 0 : 00.00";
        bestText.text = "BEST : 0 : 00.00";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLaps();
        UpdateTimers();
    }

    private void UpdateTimers()
    {
        if (timer.currentLapTime != currentLapTime)
        {
            currentLapTime = timer.currentLapTime;
            timerText.text = $"TIME : {(int)timer.currentLapTime / 60} : {(currentLapTime) % 60:00.00}";
        }

        if (timer.lastLapTime != lastLapTime)
        {
            lastLapTime = timer.lastLapTime;
            lastText.text = $"LAST : {(int)timer.lastLapTime / 60} : {lastLapTime % 60:00.00}";
        }

        if (timer.bestLapTime != bestLapTime)
        {
            bestLapTime = timer.bestLapTime;
            bestText.text = bestLapTime < 1000000 ? $"BEST : {(int)timer.bestLapTime / 60} : {(bestLapTime) % 60:00.00}" : "BEST : 0:00.00";
        }
    }

    void UpdateLaps()
    {
        lapsText.text = "L A P : " + LapsManager.instance.currentLaps + " / " + LapsManager.instance.totalLaps;
    }

    public void OnConfirmPanelOpened()
    {
        confirmPanel.SetActive(true);
        audioSource.pitch = (Random.Range(0.7f, 1.1f));
        audioSource.Play();
    }

    public void OnConfirmPanelClosed()
    {
        confirmPanel.SetActive(false);
        audioSource.pitch = (Random.Range(0.7f, 1.1f));
        audioSource.Play();
    }

    public void OnPausePanelOpened()
    {
        isPaused = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        GameObject.FindObjectOfType<CarController>().GetComponent<AudioSource>().Pause();
        GameObject.FindObjectOfType<AICarController>().GetComponent<AudioSource>().Pause();
        audioSource.PlayOneShot(pauseClip, 4f);
    }

    public void OnPausePanelClosed()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        GameObject.FindObjectOfType<CarController>().GetComponent<AudioSource>().UnPause();
        GameObject.FindObjectOfType<AICarController>().GetComponent<AudioSource>().UnPause();
        audioSource.pitch = (Random.Range(0.9f, 1.1f));
        audioSource.Play();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
        audioSource.pitch = (Random.Range(0.7f, 1.1f));
        audioSource.Play();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        audioSource.pitch = (Random.Range(0.7f, 1.1f));
        audioSource.Play();
    }
}
