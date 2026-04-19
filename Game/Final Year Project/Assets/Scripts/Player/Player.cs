using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour

{
    public DynamicDifficultyAdjustment DDA;
    //player health
    public int playerHealth = 3;

    //player movement speed
    public float playerMovement = 5f;
    //Ranks for the player
    
    public enum playerDirection { Up = 1, Down = 2, Left = 3, Right = 4 };
    public playerDirection direction;
    public float multiplier;
    // Start is called before the first frame update
    void Start()
    {
        multiplier = 1;
        direction = playerDirection.Up;
    }

    // Update is called once per frame
    void Update()
    {

        
       



    }
    
}
