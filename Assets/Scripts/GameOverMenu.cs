using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject GameOverUI;
    
    private void Awake()
    {
        GameOverUI.SetActive(false);
    }
    
    private void Update()
    {
        if (true /*dead*/)
        {
            GameOverUI.SetActive(true);
        }   
    }

    public void Restart()
    {
        // initiate another try
        GameOverUI.SetActive(false);
    }
    
    public void OpenMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}
