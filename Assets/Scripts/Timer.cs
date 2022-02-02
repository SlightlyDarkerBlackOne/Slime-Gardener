using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 120;
    public bool timerIsRunning = false;

    private TextMeshProUGUI timeText;

    private void Start() {
        // Starts the timer automatically
        timerIsRunning = true;
        timeText = gameObject.transform.Find("text").GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        if (timerIsRunning) {
            DisplayTime(timeRemaining);
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            } else {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay) {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}