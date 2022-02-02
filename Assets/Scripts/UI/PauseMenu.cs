using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool GameIsPaused;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            HandlePauseResume();
        }
    }

    public void HandlePauseResume() {
        if (GameIsPaused) {
            Resume();
        } else {
            Pause();
        }
    }

    private void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    private void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}