using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Goose : MonoBehaviour
{
    private GameObject closest;
    private Transform targetZombie;
    [SerializeField]private float offset;
    private GameObject[] zombie;
    [SerializeField]private float speed;
    [SerializeField]private GameObject gooseSound;
    NavMeshAgent agent;
    private bool zombieExists;

    private GameObject FindClosestEnemy(GameObject[] enemy) 
    {
        if(enemy != null)
        {
            float distance = Mathf.Infinity;
            Vector3 position = transform.position;
            foreach (GameObject go in enemy) 
            {
                Vector3 diff = go.transform.position - position;
                float curDistance = diff.sqrMagnitude;
                if(curDistance< distance) 
                {
                    closest = go;
                    distance = curDistance;
                }
            }
        }
        else
        {
            enemy = GameObject.FindGameObjectsWithTag("zombie");
            zombieExists = enemy.Length > 0 ? true : false;
            if (zombieExists)
                FindClosestEnemy(enemy);
        }
            
            
        
        return closest;
    }
   
    private void Start()
    {
        targetZombie = FindClosestEnemy(null).transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    private void Update()
    {
        if (zombieExists)
        {
            if (targetZombie != null) 
            { 
                agent.SetDestination(targetZombie.position);
                Vector3 targetDirection = transform.position - targetZombie.position;
                float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.Rotate(0, 0, 90);
            }   
            else 
            {
                GameObject enemy = FindClosestEnemy(null);
                if(enemy != null)
                    targetZombie = enemy.transform;
            }

                
        }
    }
}
