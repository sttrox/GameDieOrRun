using System;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public float timeDestroy = 0.8f;
    private ControllerColor _controllerColor;

    public void Start()
    {
        _controllerColor = GetComponent<ControllerColor>();
        _controllerColor.timeDamping = timeDestroy;
    }

    public void Activate()
    {
        //Debug.Log(name + " Is Activated");
        StartDestroy();
    }

    private void StartDestroy()
    {
        _controllerColor.ActivateDamping();
        Destroy(gameObject, timeDestroy);
    }
}