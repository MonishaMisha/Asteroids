using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoTimeOutQueue: IDisposable
{
    // Start is called before the first frame update
    private Dictionary<CoroutineRunner, Action> collection = new Dictionary<CoroutineRunner, Action>();
    public void Add(float timeOutDuration, Action callback, MonoBehaviour behaviour)
    {
        var runner =  new CoroutineRunner(OnTriggered, behaviour);
        collection.Add(runner, callback);
        runner.Run(timeOutDuration);
    }

    public void Remove(Action callback, MonoBehaviour behaviour)
    {
       var pair = collection.FirstOrDefault(x => x.Value == callback && x.Key.Behaviour == behaviour);
        if(pair.Value != null && pair.Key != null)
        {
            collection.Remove(pair.Key);
        }
    }

    private void OnTriggered(CoroutineRunner runner)
    {
        var isAvailable = collection.TryGetValue(runner, out Action value);
        if (isAvailable)
        {
            value?.Invoke();
        }
        collection.Remove(runner);
    }

    public void Dispose()
    {
        foreach (var pair in collection)
        {
            if (pair.Key != null)
            {
                pair.Key.Stop();
            }
        }
        collection.Clear();
    }
}

public class CoroutineRunner
{
    public Coroutine Coroutine { get; private set; }
    public Action<CoroutineRunner> Callback { get; private set; }
    public MonoBehaviour Behaviour { get; private set; }
    public CoroutineRunner(Action<CoroutineRunner> callback, MonoBehaviour behaviour)
    {
        
        Callback = callback;
        Behaviour = behaviour;
    }

    public void Run(float timeOut)
    {
        Coroutine = Behaviour?.StartCoroutine(Timer(Callback, timeOut)); ;
    }

    IEnumerator Timer(Action<CoroutineRunner> callBack, float timeOut)
    {
        yield return new WaitForSeconds(timeOut);
        callBack?.Invoke(this);
    }

    public void Stop()
    {
        if (Coroutine != null)
        {
            Behaviour?.StopCoroutine(Coroutine);
        }
    }
}