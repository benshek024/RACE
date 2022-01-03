using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public LevelChanger lvChanger;
    public AudioSource audioSource;
    public GameObject htpPanel;

    private void Start()
    {
        htpPanel.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    public void FadeToNextLevel()
    {
        lvChanger.FadeToLevel();
        audioSource.pitch = (Random.Range(0.7f, 1.1f));
        audioSource.Play();
    }

    public void ShowHTPPanel()
    {
        audioSource.pitch = (Random.Range(0.7f, 1.1f));
        audioSource.Play();
        htpPanel.SetActive(true);
    }

    public void HideHTPPanel()
    {
        audioSource.pitch = (Random.Range(0.7f, 1.1f));
        audioSource.Play();
        htpPanel.SetActive(false);
    }

    public void Quit()
    {
        audioSource.pitch = (Random.Range(0.7f, 1.1f));
        audioSource.Play();
        Application.Quit();
    }
}
