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
    [SerializeField] private GameObject toDestroy;
    [SerializeField] private bool loadSceneOnDie;
    [SerializeField] private int sceneIndex;
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private List<Shield> shields;

    private float currentHealth;

    private SpriteRenderer spriteRenderer;
    private Color defaultColor;

    [System.Serializable]
    public class Shield
    {
        [Range(0f, 100f)] public float DamageResistanceInPercents;
        public bool BreakAfterDamage;
        public float Damage;
        public float DamageRestToBreak;
        public bool BreackAfterTime;
        public float DurabilityTime;
        public float StartTime;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (healTag != "" && other.gameObject.CompareTag(healTag))
        {
            Destroy(other.gameObject);
            TakeDamage(-other.gameObject.transform.localScale.x);
        }
    }
    
    private float ShieldCalculation(float startDamage)
    {
        float takenDamage = 0;
        if (shields != null && shields.Count > 0)
        {
            foreach (Shield shield in shields)
            {
                float probablyTakenDamage = startDamage * (shield.DamageResistanceInPercents / 100);
                if(startDamage - (takenDamage + probablyTakenDamage) <= 0)
                    probablyTakenDamage = startDamage - takenDamage;

                
                if((shield.BreackAfterTime && shield.DurabilityTime <= (Time.time - shield.StartTime))
                || (shield.BreakAfterDamage && shield.DamageRestToBreak <= 0))
                {
                    shields.Remove(shield);
                    continue;
                }
                else if(shield.BreakAfterDamage && shield.DamageRestToBreak <= probablyTakenDamage)
                {
                    takenDamage += shield.DamageRestToBreak;
                    shields.Remove(shield);
                    continue;
                }
                else
                {
                    takenDamage += probablyTakenDamage;
                    shield.DamageRestToBreak -= probablyTakenDamage;
                }

                if (probablyTakenDamage <= 0)
                    break;
            }
        }
        float restDamage = startDamage - takenDamage;
        return restDamage;
    }

    public void TakeDamage(float damage)
    {
        if (shields != null && shields.Count > 0)
            currentHealth -= ShieldCalculation(damage);
        else
            currentHealth -= damage;

        if(vibration)
            StartCoroutine(SetVibration(0.2f, 0.2f));
        if (cameraController)
            cameraController.Shake(0.2f, 0.2f);

        if (currentHealth <= 0f)
        {
            if(destroyOnDie)
                Destroy(toDestroy);
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
            int currentInjureLevel = injureLevelsVisualControl.GetCurrentFrameIndex() + 2;
            int framesAmount = injureLevelsVisualControl.GetFramesAmount() + 1;
            float healthPerInjureLevel = maxHealth / framesAmount; //5.(3)
            float expectedHealthLevel = (framesAmount - currentInjureLevel) * healthPerInjureLevel;// (3-i) * 5.(3) = 16 - 5.(3)i

            if (currentHealth < expectedHealthLevel)
                injureLevelsVisualControl.SetFrame(currentInjureLevel - 3);
            else if (currentHealth - expectedHealthLevel < 0)
                injureLevelsVisualControl.SetFrame(currentInjureLevel - 1);
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

