using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

[RequireComponent(typeof(AITransform))]
public class AILogic : MonoBehaviour
{
    [SerializeField, Tooltip("Should have child 'atack point'")] private Transform anchor;
    [SerializeField] private float maxDistanceToAtack;
    [SerializeField, Tooltip("Attach if can attack")] private EntityAtack entityAtack;
    [SerializeField] private float maxDistanceToReact;
    [SerializeField, Tooltip("Attach if can fire")] private Gun gun;
    [SerializeField] private float maxDistanceToFire;
    [SerializeField] private float minDistanceToDespawn;

    private bool movementActivity, atackActiviy, gunActivity;
    

    [SerializeField] private GameObject target;

    [SerializeField, Tooltip("Something that the AI is afraid of")] private Transform[] fearObjects;

    private AITransform aiTransform;

    private void Start()
    {
        movementActivity = atackActiviy = gunActivity = false;
        aiTransform = GetComponent<AITransform>();
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        //if any of the activities isn't taken ////must be completed
        if ((!movementActivity && !atackActiviy && !gunActivity) ||
           (movementActivity && aiTransform.IsAgentArrived() && !atackActiviy && !gunActivity) ||
           (!movementActivity && atackActiviy  && entityAtack.CanAttack() && !gunActivity))
        {
            movementActivity = atackActiviy = gunActivity = false;
            DecideToActivity(distanceToTarget);
        }
            
    }

    private void DecideToActivity(float distanceToTarget)
    {
        bool seeTarget = checkIfCanSee(anchor, distanceToTarget, target.transform);

        List<Transform> seeFearObjects = new List<Transform>();
        foreach (Transform fearobject in fearObjects)
            if (checkIfCanSee(anchor, distanceToTarget, fearobject))
                seeFearObjects.Add(fearobject);

        if (distanceToTarget >= minDistanceToDespawn)
            Destroy(gameObject);
        else if (seeFearObjects.Count > 0)
        {
            float averageAngle = CalculateAverageDirection(seeFearObjects);
            if (averageAngle != 0)
                aiTransform.RotateTo(averageAngle);

            movementActivity = true;
            aiTransform.MoveTo(transform.up * 5);
        } 
        else if (distanceToTarget <= maxDistanceToAtack && seeTarget && entityAtack)
        {
            //attackp
            atackActiviy = true;
            entityAtack.MultipleTargetsAtack();
            
        }
        if (distanceToTarget <= maxDistanceToReact && seeTarget)
        {
            movementActivity = true;
            aiTransform.RotateTo(target.transform.position, 90);
            aiTransform.MoveTo(target.transform.position);
        }





        //attack
        //else if (distanceToTarget)
    }
    private float CalculateAverageDirection(List<Transform> positions)
    {
        if (positions.Count == 0)
            return 0;    

        float averageAngle = 0;

        foreach (Transform position in positions)
        {
            // Calculate the direction vector from the zombie to each dog.
            Vector3 targetDirection = transform.position - position.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
            
            // Add the direction to the total direction vector.
            averageAngle += angle;
        }

        // Divide the total direction vector by the number of dogs to get the average direction.
        averageAngle /= positions.Count;
      
        //Debug.DrawRay(transform.position, transform.up * 5f, Color.yellow);
        return averageAngle; //averaeDirection;
    }
    private Transform checkIfCanSee(Transform anchor, float distance, Transform target)
    {
        Vector3 targetDirection = anchor.position - target.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + 90;
        anchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        RaycastHit2D hit = Physics2D.Raycast(anchor.GetChild(0).position, anchor.GetChild(0).up, distance);
        Debug.DrawRay(anchor.GetChild(0).position, anchor.GetChild(0).up * distance, Color.red);

        if (hit.collider)
        {
            GameObject probablyTarget = hit.collider.gameObject;

            if (probablyTarget.tag == target.tag && probablyTarget.name == target.name)
                return target;
        }    

        return null;
    }    
}
