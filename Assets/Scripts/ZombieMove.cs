using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class ZombieMove : MonoBehaviour
{

    [SerializeField] private Transform player;
    [SerializeField] private bool isSecurity = true;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackDelay = 1f;
    [SerializeField] private AudioClip biteSound;

    private AudioSource audioSource;
    private bool canAttack = true;
    public Transform startTransform { get; private set; }
    private NavMeshAgent agent;
    [SerializeField] private LayerMask targetsLayers;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(CheckForPlayer());
    }
    private void Update()
    {
        //Ray2D ray = new Ray2D(transform.GetChild(0).transform.position, transform.up);
        //RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 5f);

        
            //if (hit.collider != null && hit.collider.CompareTag("player"))
            //{
                //player = hit.collider.gameObject.transform;
                
                
            //}
       
    }
    private IEnumerator CheckForPlayer()
    {

        for(; ; )
        {
            if (isSecurity) 
            { 
                Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 10f, targetsLayers);
                if (targets.Length != 0)
                {
                    agent.SetDestination(targets[0].transform.position);
                    Vector2 direction = targets[0].transform.position - transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 6f * Time.deltaTime);
                    if (canAttack && Vector2.Distance(transform.position, targets[0].transform.position) <= attackRange)
                        Attack(targets[0].gameObject);
                }
                
            }
            yield return new WaitForSeconds(2f);
        }
        
    }
    private void Attack(GameObject target)
    {
        canAttack = false;
        if(target.GetComponent<HealthSystem>())
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
    private void RunFrom(Transform target)
    {
        startTransform = transform;
        Vector3 targetDirection = transform.position - target.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.Rotate(0, 0, -90 +Random.Range(-20, 20));
        Vector3 runTo = transform.position + transform.up * Random.Range(1f, 10f);
        NavMeshHit hit;
        NavMesh.SamplePosition(runTo, out hit, 5, 1 << NavMesh.GetNavMeshLayerFromName("Default"));
        transform.position = startTransform.position;
        transform.rotation = startTransform.rotation;
        agent.SetDestination(hit.position);
        isSecurity = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("goose"))
        {
            isSecurity = false;
            RunFrom(collision.gameObject.transform);
        }
    }
    

}
