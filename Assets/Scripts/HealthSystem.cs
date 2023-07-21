using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    
    [SerializeField] private Image healthBar;
    [SerializeField] private Text healthCountText;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private OneFrameAnimationsControl injureLevelsVisualControl;
    [SerializeField] private string healTag;
    [SerializeField] private bool vibration;
    [SerializeField] private bool destroyOnDie = true; 
    [SerializeField] private bool loadSceneOnDie;
    [SerializeField] private int sceneIndex;
    [SerializeField] private float maxHealth = 1f;

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
        if (healTag != "" && other.gameObject.CompareTag(healTag))
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

        if(vibration)
            StartCoroutine(SetVibration(0.2f, 0.2f));
        if (cameraController)
            cameraController.Shake(0.2f, 0.2f);

        if (currentHealth <= 0f)
        {
            if(destroyOnDie)
                Destroy(gameObject);
            else if(loadSceneOnDie)
                SceneManager.LoadScene(sceneIndex);
            return;
        }

        if (spriteRenderer)
            StartCoroutine(SetColor(0.3f, new Color(defaultColor.r, defaultColor.g - 0.3f, defaultColor.b - 0.3f)));

        if (healthBar)
            healthBar.fillAmount = currentHealth / maxHealth;


        if (injureLevelsVisualControl)
        {
            int currentInjureLevel = injureLevelsVisualControl.GetCurrentFrameIndex();
            if (currentHealth > (injureLevelsVisualControl.GetFramesAmount() - (currentInjureLevel + 1)) * (maxHealth / injureLevelsVisualControl.GetFramesAmount() + 1))// first level
                injureLevelsVisualControl.SetFrame(currentInjureLevel - 1);
            else if (currentHealth < (injureLevelsVisualControl.GetFramesAmount() - (currentInjureLevel + 1)) * (maxHealth / injureLevelsVisualControl.GetFramesAmount() + 1))
                injureLevelsVisualControl.SetFrame(currentInjureLevel + 1);

        }
            



        if (healthCountText)
            healthCountText.text = "HP: " + currentHealth.ToString();

        
    }

    private IEnumerator SetColor(float duration, Color color)
    {
        spriteRenderer.color = color;
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = defaultColor;
    }
    private IEnumerator SetVibration(float duration, float force)
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(force, force + 0.5f);
            yield return new WaitForSeconds(duration);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}

