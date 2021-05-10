using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GamePlay GamePlay = null;
    public GameObject Panel = null;
    public Button Resume = null;
    public Button Exit = null;
    bool bPause = false;
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(PauseGame);
        Resume.onClick.AddListener(GameResume);
        Exit.onClick.AddListener(GameExit);

        Panel.SetActive(false);
    }
    public void PauseGame()
    {
        if (!bPause)
        {
            Panel.SetActive(true);

            Time.timeScale = 0;
            bPause = true;
        }
        else
        {
            Panel.SetActive(false);

            Time.timeScale = 1;
            bPause = false;
        }
    }

    void GameResume()
    {
        PauseGame();
    }

    void GameExit()
    {
        PauseGame();
        GamePlay.Restart();
    }
}
