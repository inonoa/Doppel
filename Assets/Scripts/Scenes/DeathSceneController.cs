using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class DeathSceneController : MonoBehaviour
{
    [SerializeField] DangeonController dangeonController;
    [SerializeField, Multiline(5)] string tweetText;

    DeathView view;

    bool canTweet = false;

    int lastFloor;

#if UNITY_WEBGL
    [DllImport("__Internal")] private static extern void OpenNewWindow(string url);
#endif

    public IObservable<Unit> Enter(int floor)
    {
        this.lastFloor = floor;
        IObservable<Unit> viewCompleted = view.Enter(floor);
        viewCompleted.Subscribe(_ => canTweet = true);
        return viewCompleted;
    }

    public void Exit()
    {
        canTweet = false;
        dangeonController.Enter();
        view.Exit();
    }

    void Tweet()
    {
        string txt = tweetText.Replace("[floor]", lastFloor.ToString());
        string url = "https://twitter.com/intent/tweet?text=" 
                     + UnityWebRequest.EscapeURL(txt);

        //ひっぱってくる
#if UNITY_WEBGL && !UNITY_EDITOR
        OpenNewWindow(url);
#else
        Application.OpenURL(url);
#endif
    }

    void Start()
    {
        view = GetComponent<DeathView>();
        view.TweetButtonPushed
        .Where(_ => canTweet)
        .Subscribe(_ => Tweet());

        view.RetryButtonPushed
        .Where(_ => canTweet)
        .Subscribe(_ => Exit());
    }


    void Update()
    {
        
    }
}
