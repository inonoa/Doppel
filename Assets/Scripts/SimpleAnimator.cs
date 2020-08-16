using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimator : MonoBehaviour
{
    [SerializeField] SimpleAnim[] anims;
    [SerializeField] SpriteRenderer spriteRenderer;

    SimpleAnim current;

    public void Play(string key)
    {
        if(current != null) current.Stop(this);
        current = Find(key);
        current.Play(spriteRenderer, this);
    }

    SimpleAnim Find(string key)
    {
        foreach(var anim in anims)
        {
            if(anim.Name == key) return anim;
        }
        return null;
    }

    void Update()
    {
        
    }
}
