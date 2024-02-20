using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float damping;

    // Camera shake parameters
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.1f;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        // Add any initialization code here
    }

    private void LateUpdate()
    {
        // Camera follow
        Vector3 movePosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocity, damping);
    }
    public void CameraShake(){
        StartCoroutine(ShakeCoroutine());
    }
    private IEnumerator ShakeCoroutine()
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-0.1f, 0.1f) * shakeMagnitude;
            float y = Random.Range(-0.1f, 0.1f) * shakeMagnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        //transform.localPosition = originalPosition;
    }
}
