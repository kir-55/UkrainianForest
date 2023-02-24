using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]private Transform position;
    [SerializeField]private float speed;
    private void Update()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,new Vector3(position.position.x,position.position.y,gameObject.transform.position.z), Time.deltaTime * speed);
    }
}
