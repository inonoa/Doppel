using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UniRx;
using System.Linq;

public class TurnController : SerializedMonoBehaviour
{
    List<Obj_done> objs_done = new List<Obj_done>();

    public void AddObj(IUnderTurns obj)
    {
        Obj_done obj_done = new Obj_done(obj, false); // falseなんか？
        objs_done.Add(obj_done);
        obj.Done.Subscribe(_ => obj_done.done = true);
    }

    void Start()
    {
        
    }


    void Update()
    {
        if(objs_done.All(od => od.done))
        {
            print("ターン開始！");
            objs_done.ForEach(od => od.done = false);
            objs_done.ForEach(od => od.Obj.OnTurnStart());
        }
    }

    class Obj_done
    {
        IUnderTurns _Obj;
        public IUnderTurns Obj => _Obj;
        public bool done;
        public Obj_done(IUnderTurns obj, bool done)
        {
            this._Obj = obj;
            this.done = done;
        }
    }
}

public interface IUnderTurns
{
    IObservable<Unit> Done{ get; }
    void OnTurnStart();
}
