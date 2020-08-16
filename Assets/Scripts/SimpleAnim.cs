using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class SimpleAnim
{
    [SerializeField] string _Name;
    public string Name => _Name;
    [SerializeField] float secsPerSprite = 0.3f;
    [SerializeField] Sprite[] sprites;

    IEnumerator cor;
    public void Play(SpriteRenderer renderer, MonoBehaviour coroutinePlayer)
    {
        cor = Anim(renderer);
        coroutinePlayer.StartCoroutine(cor);
    }

    public void Stop(MonoBehaviour coroutinePlayer)
    {
        if(cor != null) coroutinePlayer.StopCoroutine(cor);
    }

    IEnumerator Anim(SpriteRenderer renderer)
    {
        while(true)
        {
            foreach(Sprite spr in sprites)
            {
                renderer.sprite = spr;
                yield return new WaitForSeconds(secsPerSprite);
            }
        }
    }
}
