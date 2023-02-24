using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthCountText;
    [SerializeField] private bool hpSun;
    [SerializeField] private bool destroyOnDie = true;
    private float currentHealth;
    private bool shield;
    private float damageResistance, shieldEndTime;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;

    private void Start()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (shield && Time.time >= shieldEndTime)
            shield = false;
        
    }

    public void CreateShield(int resistance, float duration)
    {
        shield = true;
        damageResistance = resistance;
        shieldEndTime = Time.time + duration;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hpSun && other.gameObject.CompareTag("HpSun"))
        {
            Destroy(other.gameObject);
            TakeDamage(-other.gameObject.transform.localScale.x);
        }
    }

    public void TakeDamage(float damage)
    {
        if (shield)
            currentHealth -= damage - damage / 100 * damageResistance;
        else
            currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            if(destroyOnDie)
            Destroy(gameObject);
            return;
        }

        if (spriteRenderer)
            StartCoroutine(SetColor(0.3f, new Color(defaultColor.r, defaultColor.g - 0.3f, defaultColor.b - 0.3f)));

        if (healthBar)
            healthBar.fillAmount = currentHealth / maxHealth;

        if (healthCountText)
            healthCountText.text = "HP: " + currentHealth.ToString();
    }

    private IEnumerator SetColor(float duration, Color color)
    {
        spriteRenderer.color = color;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = defaultColor;
    }
}

