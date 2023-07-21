using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class ZombieMove : MonoBehaviour
{

    private NavMeshAgent agent;

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 runTo;
    [SerializeField] private bool isSecure = true;
    [SerializeField] private LayerMask targetsLayers;

    public Transform startTransform { get; private set; }
    
    

    private void Start()
    {
        runTo = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (isSecure)
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 5f, targetsLayers);
            if (targets.Length != 0)
            {
                agent.SetDestination(targets[0].gameObject.transform.position);
                RotateTo(targets[0].transform.position, 90);
            }
        }
    }

    private void RunFrom(Transform target)
    {
        

        RotateTo(target.position, -90);

        if (runTo == Vector3.zero)
            runTo = transform.position + transform.up * Random.Range(1f, 10f);

        agent.SetDestination(runTo);

        if(Vector3.Distance(transform.position, runTo) < 1f )
        {
            isSecure = true;
            runTo = Vector3.zero;
        }
    }
    private void RotateTo(Vector3 targetPosition, float offset)
    {
        Vector3 targetDirection = transform.position - targetPosition;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + offset;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("goose"))
        {
            isSecure = false;
            RunFrom(collision.gameObject.transform);
        }
    }
    

}
