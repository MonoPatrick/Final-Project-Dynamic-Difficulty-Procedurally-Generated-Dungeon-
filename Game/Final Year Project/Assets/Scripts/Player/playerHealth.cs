using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public Image healthBar;
    public bool debug;
    public GameObject popUpPrefab;

    private DynamicDifficultyAdjustment m_dda;
    private bool m_canTakeDamage = true;
    private SpriteFlash m_spriteFlash;

    void Start()
    {
        maxHealth = currentHealth;
        m_spriteFlash = GetComponent<SpriteFlash>();

        if (m_dda == null)
        {
            m_dda = GameObject.FindGameObjectWithTag("Difficulty").GetComponent<DynamicDifficultyAdjustment>();
        }
    }

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
        if (collision.gameObject.tag == "Projectile" && m_canTakeDamage)
        {
            DynamicDifficultyAdjustment.Instance.changeDifficulty(-0.05f);

            if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank1)
            {
                ChangeHealth(-15 * m_dda.difficulty);
            }
            if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank2)
            {
                ChangeHealth(-12 * m_dda.difficulty);
            }
            if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank3)
            {
                ChangeHealth(-10 * m_dda.difficulty);
            }
            if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank4)
            {
                ChangeHealth(-10 * m_dda.difficulty);
            }
            if (DynamicDifficultyAdjustment.Instance.playerRank == DynamicDifficultyAdjustment.Rank.Rank5)
            {
                ChangeHealth(-10 * m_dda.difficulty);
            }


            Invulnerability(1.0f);
        }

        if (collision.gameObject.tag == "Health")
        {
            ChangeHealth(40);
            DynamicDifficultyAdjustment.Instance.changeDifficulty(-0.01f);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Coin")
        {
            FindObjectOfType<ScoreCount>().AddScore(1000);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "GreaterHealth")
        {
            ChangeHealth(80);
            DynamicDifficultyAdjustment.Instance.changeDifficulty(-0.06f);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "bundleOfCoins")
        {
            FindObjectOfType<ScoreCount>().AddScore(10000);
            Destroy(collision.gameObject);
        }
    }

    public void Invulnerability(float duration)
    {
        StartCoroutine(InvulnerabilityTimer(duration, new Color(0f, 0f, 0f, 0f), 5));
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

    public bool GetCanTakeDamage()
    {
        return m_canTakeDamage;
    }

    IEnumerator InvulnerabilityTimer(float duration, Color flashColor, int numberOfFlashes)
    {
        m_canTakeDamage = false;
        yield return StartCoroutine(m_spriteFlash.FlashCoroutine(duration, flashColor, numberOfFlashes));
        m_canTakeDamage = true;
    }
}