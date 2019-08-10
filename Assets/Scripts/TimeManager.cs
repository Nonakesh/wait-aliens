using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    public Button PauseButton;
    public Sprite ClockSprite;
    public Sprite PlaySprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        if (PauseButton.image.sprite == ClockSprite)
        {
            PauseButton.image.sprite = PlaySprite;
        }
        else
        {
            PauseButton.image.sprite = ClockSprite;
        }
        
        Debug.Log("Slowing down time and pausing...");
    }
}
