using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text score;
    private int scoreCount = 0;
    private int enemieskilledCount;
    public static ScoreDisplay Instance;

    public bool enemiesKilled = false;
    void Awake()
    {
        score = GetComponent<TMP_Text>(); // automatically grab itself
        
    }
    void Start()
    {
        
        UpdateScore(ScoreCount.Instance.score);
        
    }

    public void AddScoreToDisplay(int scoreToSet)
    {
        if (!enemiesKilled)
        {
            scoreCount += scoreToSet;
            score.text = "Score: " + scoreCount.ToString();
        }
        if (enemiesKilled)
        {
            score.text = "Enemies Defeated: " + enemieskilledCount.ToString();
        }
    }
    public void AddEnemyKilled(int killed)
    {
        enemieskilledCount += killed;
    }

    public void UpdateScore(int newScore)
    {
        if (!enemiesKilled)
        {
            scoreCount = newScore;
            score.text = "Score: " + scoreCount.ToString();
        }
        if (enemiesKilled)
        {
            score.text = "Enemies killed: " + ScoreCount.Instance.enemieskilled.ToString();
        }
    }
}
