using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float posDamp = 2; //2
    public float rotDamp = 3; //3
    public float height = 2; //2
    public float distance = 5; //5

    private Vector3 targetPos;

    // Update is called once per frame
    void Update()
    {
        targetPos = target.position - target.forward * distance + target.up * height;
        transform.position = Vector3.Lerp(transform.position, targetPos, posDamp * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotDamp * Time.deltaTime);
    }
}