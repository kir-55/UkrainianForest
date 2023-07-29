using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class AITransform : MonoBehaviour
{

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public void RotateTo(Vector3 targetPosition, float offset)
    {
        Vector3 targetDirection = transform.position - targetPosition;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + offset;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public void RotateTo(float angle)
    {
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    public void MoveTo(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }
}
