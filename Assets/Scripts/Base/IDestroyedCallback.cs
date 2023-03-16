using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to get trigger callback when destroyed
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDestroyedCallback <T>
{
    void RegisterDestroyedCallback(Action<T> action);
}
