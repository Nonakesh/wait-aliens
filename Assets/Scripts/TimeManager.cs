using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static bool Paused = false;
    
    [Header("Pause Button")]
    public Button PauseButton;
    public Sprite ClockSprite;
    public Sprite PlaySprite;

    [Header("Construction Menu")] 
    public GameObject constructionScrollView;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) /*&& ResourceManager.IsAvailable(ResourceType.Time, 1)*/)
        {
            PauseOrResume();
        }/*
        else if (!ResourceManager.IsAvailable(ResourceType.Time, 1))
        {
            Paused = false;
            constructionScrollView.SetActive(false);
        }*/
    }

    public void PauseOrResume()
    {
        if (Paused)
        {
            Paused = false;
            constructionScrollView.SetActive(false);
            PauseButton.image.sprite = ClockSprite;
            Time.timeScale = 1;
        }
        else
        {
            Paused = true;
            constructionScrollView.SetActive(true);
            PauseButton.image.sprite = PlaySprite;
            Time.timeScale = 0;
        }
    }
}
