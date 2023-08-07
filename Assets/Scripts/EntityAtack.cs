using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAtack : MonoBehaviour
{

    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private LayerMask targetsLayers;
    private AudioSource audioSource;
    private bool canAttack = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MultipleTargetsAtack()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, attackRange, targetsLayers);
        if (targets.Length != 0 && canAttack)
        {
            foreach (Collider2D target in targets)
                if (Vector2.Distance(transform.position, target.transform.position) <= attackRange)
                {
                    Atack(target.gameObject);
                    if (attackSound) 
                    {
                        audioSource.PlayOneShot(attackSound);
                        audioSource.transform.position = transform.position;
                        audioSource.transform.rotation = transform.rotation;
                    }
                    
                }

            Invoke("ResetAttack", attackDelay);
        }
            

    }

    private void Atack(GameObject target)
    {
        if (target.GetComponent<HealthSystem>())
            target.GetComponent<HealthSystem>().TakeDamage(attackDamage);
 
        canAttack = false;
    }


    private void ResetAttack()
    {
        canAttack = true;
    }
    public bool CanAttack() => canAttack;
    
}
