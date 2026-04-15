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

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
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
        if (playerHealth >=3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        else if (playerHealth == 2)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(false);
        }
        else if (playerHealth >= 1)
        {
            heart1.SetActive(true);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }
        else if (playerHealth <= 0)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
        }

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
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Collided with an enemy!");
            // Perform action here
        }

    }
}
