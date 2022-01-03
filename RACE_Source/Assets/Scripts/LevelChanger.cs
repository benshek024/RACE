using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [Header("Level Changer")]
    public Animator levelChanger;

    // Start is called before the first frame update
    void Start()
    {
        levelChanger = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.instance.PlayerWin();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameManager.instance.PlayerLose();
        }
    }

    public void FadeToNextLevel()
    {
        FadeToLevel();
        UIManager.instance.audioSource.pitch = (Random.Range(0.7f, 1.1f));
        UIManager.instance.audioSource.Play();
    }

    public void FadeToLevel()
    {
        levelChanger.SetTrigger("FadeOut");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
