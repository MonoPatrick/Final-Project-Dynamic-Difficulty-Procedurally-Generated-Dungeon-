using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
   public void StartGame()
    {
        // Load the next scene or the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame"); 
    }
    public void Instructions()
    {
        // Load the next scene or the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
