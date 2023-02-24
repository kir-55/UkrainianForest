using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]private float offset;
    [SerializeField]private float speed, rotationSpeed;
    private Rigidbody2D rb;
    [SerializeField]private int maxLife;
    [SerializeField]private Image healthBar;

    private int life;
    private bool dubleClick;
    private float clickTime = 0;

    GameControler gameControler;
    private Vector2 move, rotation;

    // private void Awake()
    // {
    //     gameControler = new GameControler();

    //     gameControler.Gamepad.Shoot.performed += ctx => move = ctx.ReadValue<Vector2>();
    //     gameControler.Gamepad.Move.canceled += ctx => move = Vector2.zero;
    // }

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
        
        Debug.Log(GetComponent<PlayerInput>().currentControlScheme);
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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        life = maxLife;
    }

    //  void OnEnable()
    // {
    //     gameControler.Gamepad.Enable();
    // }

    // void OnDisable()
    // {
    //     gameControler.Gamepad.Disable();
    // }

    private void FixedUpdate()
    {
        MoveLogicMethod();

    //     Vector2 m = new Vector2(-move.x, move.y) * Time.deltaTime;
    //     transform.Translate(m, Space.World);
        //MOVE
        //input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        
        // if(input != null)
        // {
        //     if ((Time.realtimeSinceStartup - clickTime) < 0.3f)
        //     {
        //         speed = 3;
        //     }
        //     else
        //         speed = 2;
        //     clickTime = Time.realtimeSinceStartup;
        // }
        
        // if(dubleClick)
        //     speed = 3; 
        // else
        //     speed = 2;
        // Vector3 velocity = input.normalized * speed;
        // transform.position += velocity * Time.deltaTime;
        // Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // float rotateZ = Mathf.Atan2(difference.y,difference.x)* Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0f,0f,rotateZ + offset);
    }

    
    public void Damage(int damage)
    {
        life -= damage;
    }
}