using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject loadMenuUI;
    public GameObject loseScreenUI;
    public static bool GameIsPaused;

    private void Start() {
        PlayerHealthManager.Instance.OnPlayerDead += Pause;
        PlayerHealthManager.Instance.OnPlayerDead += ShowLoseScreen;
    }
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
        SFXManager.Instance.PlaySound(SFXManager.Instance.uiButtons[0]);
    }

    private void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SFXManager.Instance.PlaySound(SFXManager.Instance.uiButtons[1]);
    }

    public void LoadMenu() {
        loadMenuUI.SetActive(!loadMenuUI.activeSelf);
        SFXManager.Instance.PlaySound(SFXManager.Instance.winGame);
    }
    private void QuitGame() {
        Application.Quit();
    }
    private void ShowLoseScreen() {
        loseScreenUI.gameObject.SetActive(true);
        SFXManager.Instance.StopSoundTrack();
    }
    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}