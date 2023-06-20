using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 targetPos = player.position;
        targetPos.z = transform.position.z;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
    public void Shake(float duration = 0.2f, float magnitude = 0.2f)
    {
        StartCoroutine(ShakeCorutine(duration, magnitude));
    }

    public IEnumerator ShakeCorutine(float duration = 0.2f, float magnitude = 0.2f)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
