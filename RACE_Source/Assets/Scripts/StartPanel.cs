using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip beep;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        GameManager.instance.startGame = true;
        gameObject.SetActive(false);
    }

    public void PlayBeep()
    {
        audioSource.PlayOneShot(beep, 2f);
    }
}
