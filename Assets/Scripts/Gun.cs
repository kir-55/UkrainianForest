using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField]private Transform shotDir;
    [SerializeField]private GameObject bullet;
    [SerializeField]private GameObject shotSound;
    GameControler gameControler;

    private void Awake()
    {
        gameControler = new GameControler();
        gameControler.Gamepad.Shoot.performed += ctx => Shoot();
    }

    private void Update()
    {
        // if(Input.GetMouseButtonDown(0))
        // {
        //     Shoot();
        // }
    }

    private void Shoot()
    {
        Instantiate(bullet,shotDir.position,shotDir.rotation);
        Instantiate(shotSound,shotDir.position,shotDir.rotation);
    }

    void OnEnable()
    {
        gameControler.Gamepad.Enable();
    }

    void OnDisable()
    {
        gameControler.Gamepad.Disable();
    }
}
