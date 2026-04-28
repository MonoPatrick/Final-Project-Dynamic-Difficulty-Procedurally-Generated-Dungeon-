using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDifficultyAdjustment : MonoBehaviour
{
    public static DynamicDifficultyAdjustment Instance;

    public float difficulty;
    public enum Rank { Rank1 = 1, Rank2 = 2, Rank3 = 3, Rank4 = 4, Rank5 = 5 };
    public Rank playerRank;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //  survives scene reload
        }
        else
        {
            Destroy(gameObject); // prevent duplicates
        }
    }
    private void Start()
    {
        if (difficulty == 0f) // only set default once
        {
            difficulty = 1f;
        }

        correctingDifficulty();
        difficultyRankSet();
    }
    private void Update()
    {
        correctingDifficulty();
        settingRank();


    }
    void correctingDifficulty()
    {
        if (difficulty < 0.5f)
        {
            difficulty = 0.5f;
        }
        else if (difficulty > 2.0f)
        {

            difficulty = 2.0f;

        }
    }

    void difficultyRankSet()
    {
        if (playerRank == Rank.Rank1)
        {
            difficulty = 0.5f;
        }
        if (playerRank == Rank.Rank2)
        {
            difficulty = 0.76f;
        }
        if (playerRank == Rank.Rank3)
        {
            difficulty = 1.0f;
        }
        if (playerRank == Rank.Rank4)
        {
            difficulty = 1.5f;
        }
        if (playerRank == Rank.Rank5)
        {
            difficulty = 2f;
        }
    }
    void settingRank()
    {
        if (difficulty <= 0.75f && difficulty >= 0.50f)
        {
            playerRank = Rank.Rank1;
        }
        else if (difficulty <= 0.99f && difficulty >= 0.76f)
        {
            playerRank = Rank.Rank2;
        }
        else if (difficulty <= 1.35f && difficulty >= 1.0f)
        {
            playerRank = Rank.Rank3;
        }
        else if (difficulty <= 1.7f && difficulty >= 1.36f)
        {
            playerRank = Rank.Rank4;
        }
        else if (difficulty <= 2.0f && difficulty >= 1.70f)
        {
            playerRank = Rank.Rank5;
        }
    }
    public void changeDifficulty(float amount)
    {
        difficulty += amount;
    }

    
}

