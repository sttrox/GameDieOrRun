using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;
    public float smooth = 5.0f;

    void Update()
    {
        transform.position =
            Vector3.Lerp(transform.position, target.transform.position + offset, smooth * Time.deltaTime);
    }
}