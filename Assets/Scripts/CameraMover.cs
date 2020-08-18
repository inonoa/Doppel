using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    FloorStatus status;
    [SerializeField] Vector3 offset;

    public void Init(FloorStatus status)
    {
        this.status = status;
    }

    void Start()
    {
        
    }


    void Update()
    {
        if(status != null && status.hero != null && status.hero.enabled) transform.position = status.hero.transform.position + offset;
    }
}
