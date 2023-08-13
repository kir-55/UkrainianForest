using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{ 
    [SerializeField] private GameObject player;
    [SerializeField] private float mobsSpawnDelay;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private GameObject[] mobs;
    //[SerializeField] private DayNightCycle dayNightCycle;
    /// <summary>
    /// public static event TimeAction OnDay;
    /// </summary>
    ///public static event TimeAction OnNight;
    //public static UnityEvent OnDay, OnNight;


    private void OnEnable()
    {
        DayNightCycle.OnDay += IsDay;
        DayNightCycle.OnNight += IsNight;
    }
    private void IsDay()
    {
        print("day");
        StopCoroutine("SpawnMobs");
    }
    private void IsNight()
    {
        print("night");
        StartCoroutine("SpawnMobs");
    }
    private IEnumerator SpawnMobs()
    {
        for(; ; )
        {
            yield return new WaitForSeconds(mobsSpawnDelay);
            if (mobs.Length > 0)
            {
                GameObject mob = mobs[Random.Range(0, mobs.Length)];
                Vector2 playerPos = player.transform.position;
                Vector2 positonInCircle = Vector2.zero;
                //Quaternion.Euler(0.0, 0.0, Random.Range(0.0, 360.0)
                bool isPointFarAwayFromPlayer = false;
                while (isPointFarAwayFromPlayer == false)
                { 
                    positonInCircle = Random.insideUnitCircle.normalized;
                    isPointFarAwayFromPlayer = Square(positonInCircle.x - playerPos.x) + Square(positonInCircle.y - playerPos.y) >= 0.5f;
                }
               
                Vector3 posToSpawn = playerPos + positonInCircle * distanceFromPlayer;
                mob = Instantiate(mob, posToSpawn, Quaternion.identity, transform);
                if (mob.GetComponent<AILogic>())
                    mob.GetComponent<AILogic>().target = player;
                print("mob spavned: " + Random.insideUnitCircle.normalized);
            }
        }
        
        
    }
    private float Square(float num) => num * num;
    private void OnDisable()
    {
        DayNightCycle.OnDay -= IsDay;
        DayNightCycle.OnNight -= IsNight;
    }
}
