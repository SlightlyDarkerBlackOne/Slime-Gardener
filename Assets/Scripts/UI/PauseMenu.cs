using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    bool paused;
    // Start is called before the first frame update
    void Start() {
        paused = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Escape) && (paused == false)) {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            //paused = true;
        }
        if (Input.GetKey(KeyCode.Escape) && (paused == true)) {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        }
    }
}