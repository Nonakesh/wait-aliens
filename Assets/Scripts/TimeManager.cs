using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static bool Paused = false;
    
    [Header("Pause Button")]
    public Button PauseButton;
    public Sprite PauseSprite;
    public Sprite PlaySprite;

    [Header("Construction Menu")] 
    public GameObject constructionScrollView;

    private float timePaused;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PauseOrResume();
        }

        if (Paused)
        {
            timePaused += Time.unscaledDeltaTime;

            if (ResourceManager.GetResource(ResourceType.Time) == 0)
            {
                Unpause();
            }
            
            if (timePaused > 1)
            {
                var amount = (int) timePaused;
                if (ResourceManager.TryTakeResource(ResourceType.Time, amount))
                {
                    timePaused -= amount;
                }
                else
                {
                    timePaused = 0;
                    Unpause();
                }
            }
        }
    }

    public void PauseOrResume()
    {
        if (Paused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        Paused = true;
        constructionScrollView.SetActive(true);
        PauseButton.image.sprite = PlaySprite;
        Time.timeScale = 0;
    }

    private void Unpause()
    {
        Paused = false;
        constructionScrollView.SetActive(false);
        PauseButton.image.sprite = PauseSprite;
        Time.timeScale = 1;
    }
}
