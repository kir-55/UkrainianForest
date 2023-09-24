using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float speed,damage;
    [SerializeField]private GameObject normalParticle, bloodParticle;
    
    private void Update()
    {
        transform.Translate(Vector2.up * speed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<HealthSystem>())
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
            Instantiate(bloodParticle, transform.position, transform.rotation);
        }    
        else
            Instantiate(normalParticle, transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
