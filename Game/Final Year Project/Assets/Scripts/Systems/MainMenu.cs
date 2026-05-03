using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool settingDifficulty = false;
    public GameObject mainMenu;
    public GameObject difficultySelect;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        difficultySelect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingDifficulty)
            {
                mainMenu.SetActive(true);
                difficultySelect.SetActive(false);
                settingDifficulty = false;
            }
            else
            {
                Application.Quit();
            }
        }
    }
    
   public void StartGame()
    {
        // Load the next scene or the main game scene
        mainMenu.SetActive(false);
        difficultySelect.SetActive(true);
        settingDifficulty = true;
    }
    public void Instructions()
    {
        // Load the next scene or the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Instructions");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetEasy()
    {
        GameSettings.easy = true;
        GameSettings.normal = false;
        GameSettings.hard = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");

    }

    public void SetNormal()
    {
        GameSettings.normal = true;
        GameSettings.easy = false;
        GameSettings.hard = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }

    public void SetHard()
    {
        GameSettings.normal = false;
        GameSettings.easy = false;
        GameSettings.hard = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }
}
