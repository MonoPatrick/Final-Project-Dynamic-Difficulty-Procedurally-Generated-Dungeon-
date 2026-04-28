using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneReloader : MonoBehaviour
{
    string currentSceneName;
    bool clicked;
    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = "";
    }

    // Update is called once per frame
    void Update()
    {
        //if clicked == true)
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "End")
        {
            
            sceneReload();
        }
    }

    public void sceneReload()
    {
        FindObjectOfType<ScoreCount>().AddScore(10000);
        DynamicDifficultyAdjustment.Instance.changeDifficulty(0.1f);
        currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
