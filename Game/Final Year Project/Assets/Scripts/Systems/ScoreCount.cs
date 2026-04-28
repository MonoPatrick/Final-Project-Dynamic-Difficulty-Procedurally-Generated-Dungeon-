using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCount : MonoBehaviour
{
    public static ScoreCount Instance;
    public int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;

        FindObjectOfType<ScoreDisplay>().UpdateScore(score);
    }
    private void Update()
    {
        //FindObjectOfType<ScoreDisplay>().UpdateScore(score);
    }

}
