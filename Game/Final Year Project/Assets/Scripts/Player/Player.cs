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
    // Start is called before the first frame update
    void Start()
    { 
        playerRank = Rank.Rank1;
        direction = playerDirection.Up;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
