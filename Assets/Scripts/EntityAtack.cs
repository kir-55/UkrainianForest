using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAtack : MonoBehaviour
{

    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private AudioClip biteSound;
    [SerializeField] private LayerMask targetsLayers;
    private AudioSource audioSource;
    private bool canAttack = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void CheckForAtack()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 5f, targetsLayers);
        if (targets.Length != 0)
        {
            if (canAttack && Vector2.Distance(transform.position, targets[0].transform.position) <= attackRange)
                Atack(targets[0].gameObject);
        }
    }

    public void Atack(GameObject target)
    {
        canAttack = false;
        if (target.GetComponent<HealthSystem>())
            target.GetComponent<HealthSystem>().TakeDamage(attackDamage);
        Invoke("ResetAttack", attackDelay);
        audioSource.PlayOneShot(biteSound);
        audioSource.transform.position = transform.position;
        audioSource.transform.rotation = transform.rotation;
    }


    private void ResetAttack()
    {
        canAttack = true;
    }
}
