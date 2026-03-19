using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{
    //player health
    public int playerHealth = 3;

    //player movement speed
    public float playerMovement = 5f;
    //Ranks for the player
    public enum Rank { Rank1 = 1,Rank2 = 2 ,Rank3 = 3,Rank4 = 4,Rank5 = 5};
    public enum playerDirection { Up = 1, Down = 2, Left = 3, Right = 4 };
    public Rank playerRank;
    public playerDirection direction;

    public float multiplier;
    // Start is called before the first frame update
    void Start()
    {
        multiplier = 1;
        playerRank = Rank.Rank1;
        direction = playerDirection.Up;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRank == Rank.Rank1)
        {
            multiplier = 0.5f;
        }
        if (playerRank == Rank.Rank2)
        {
            multiplier = 0.75f;
        }
        if(playerRank == Rank.Rank3)
        {
            multiplier = 1f;
        }
        if (playerRank == Rank.Rank4)
        {
            multiplier = 1.5f;
        } 
        if (playerRank == Rank.Rank5)
        {
            multiplier = 2f;
        }

    }
}
