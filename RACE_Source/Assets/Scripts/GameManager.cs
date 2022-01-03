using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance { get { return _instance; } }

    [Header("SFX")]
    public AudioSource audioSource;
    public AudioClip lapPassedClip;
    public AudioClip winClip;

    [Header("Booleans")]
    public bool startGame = false;

    private void Awake()
    {
        // Singleton for GameManager
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
        startGame = false;
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        StartCoroutine(Restart(2f));
    }

    IEnumerator Restart(float resetTimer)
    {
        yield return new WaitForSeconds(resetTimer);
        SceneManager.LoadScene(0);
    }

    public void PlayerWin()
    {
        GameObject.FindObjectOfType<CarController>().GetComponent<AudioSource>().Pause();
        GameObject.FindObjectOfType<AICarController>().GetComponent<AudioSource>().Pause();
        UIManager.instance.winPanel.SetActive(true);
        audioSource.PlayOneShot(winClip, 0.5f);
        startGame = false;
    }

    public void PlayerLose()
    {
        GameObject.FindObjectOfType<CarController>().GetComponent<AudioSource>().Pause();
        GameObject.FindObjectOfType<AICarController>().GetComponent<AudioSource>().Pause();
        UIManager.instance.losePanel.SetActive(true);
        startGame = false;
    }
}
