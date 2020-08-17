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
        if(status != null) transform.position = status.hero.transform.position + offset;
    }
}
