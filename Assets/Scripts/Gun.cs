using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform shotDir;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject shotSound;
    [SerializeField] private bool vibrations;
    [SerializeField] private bool IsPlayer;
    [SerializeField] private CameraController cameraController;
    GameControler gameControler;

    private void Awake()
    {
        if (IsPlayer)
        {
            cameraController = transform.parent.parent.GetComponent<CurrentItem>().GetCameraController();
            gameControler = new GameControler();
            gameControler.Gamepad.Use.performed += ctx => Shoot();
        }
        
    }

    public void Shoot()
    {
        Instantiate(bullet,shotDir.position,shotDir.rotation);
        Instantiate(shotSound,shotDir.position,shotDir.rotation);
        if (vibrations)
            StartCoroutine(SetVibration(0.1f, 0.3f));
        if (cameraController)
            cameraController.Shake(0.1f, 0.1f);
    }
    private IEnumerator SetVibration(float duration, float force)
    {
        if(Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(force, force + 0.5f);
            yield return new WaitForSeconds(duration);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
       
    }
    void OnEnable()
    {
        if (gameControler != null)
            gameControler.Gamepad.Enable();
    }

    void OnDisable()
    {
        if (gameControler != null)
            gameControler.Gamepad.Disable();
    }
}
