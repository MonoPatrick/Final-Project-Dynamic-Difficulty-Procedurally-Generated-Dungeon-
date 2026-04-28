using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TMP_Text score;
    private int scoreCount = 0;
    public static ScoreDisplay Instance;

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
        scoreCount += scoreToSet;
        score.text = "Score: " + scoreCount.ToString();
    }

    public void UpdateScore(int newScore)
    {
        scoreCount = newScore;
        score.text = "Score: " + scoreCount.ToString();
    }
}
