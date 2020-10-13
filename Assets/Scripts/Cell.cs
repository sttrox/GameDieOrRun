﻿using System;
using UnityEngine;

public class S_Cell : MonoBehaviour
{
    public float timeDestroy = 0.8f;

    public void Activate()
    {
        Debug.Log(name + " Is Activated");
        StartDestroy();
    }

    private void StartDestroy()
    {
        Destroy(gameObject, timeDestroy);
    }
}