using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class playerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public Image healthBar;
    private DynamicDifficultyAdjustment DDA;
    public bool debug;
    public GameObject popUpPrefab;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = currentHealth;
        if (DDA == null)
        {
            DDA = GameObject.FindGameObjectWithTag("Difficulty").GetComponent<DynamicDifficultyAdjustment>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
        if (currentHealth <= 0)
        {
            if (debug)
            {

            }
            else
            {
                SceneManager.LoadScene("GameOver");
               
               
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Projectile")
        {
            DynamicDifficultyAdjustment.Instance.changeDifficulty(-0.06f);
            ChangeHealth(-10 * DDA.difficulty);
            //Debug.Log("Collided with an enemy!");
            // Perform action here
        }
        if (collision.gameObject.tag == "Health")
        {
            ChangeHealth(40);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Coin")
        {
            FindObjectOfType<ScoreCount>().AddScore(1000);
            Destroy(collision.gameObject);
        }

    }
    public void ChangeHealth(float amount)
    {
        int popUpAmount = (int)amount;
        GameObject popUp = Instantiate(popUpPrefab, transform.position, Quaternion.identity);
        popUp.GetComponentInChildren<TMP_Text>().text = popUpAmount.ToString();
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

    }
}
