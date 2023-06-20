using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]private float offset;
    [SerializeField]private float speed, rotationSpeed;
    [SerializeField]private int maxLife;
    [SerializeField]private Image healthBar;

    private Vector2 move, rotation;
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

     private void OnRotate(InputValue value)
     {
        rotation = value.Get<Vector2>();
        if(GetComponent<PlayerInput>().currentControlScheme == "Keyboard")
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(new Vector3(rotation.x, rotation.y, 0)) - transform.position;
            rotation = new Vector2(difference.x, difference.y);
        }
     }

    private void MoveLogicMethod()
    {
        Vector2 result = move * speed * Time.deltaTime;
        rb.velocity = result;
        
        if (rotation != Vector2.zero)
        {
            var angle = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }
    }

    private void FixedUpdate()
    {
        MoveLogicMethod();
    }
}