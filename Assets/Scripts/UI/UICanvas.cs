using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class to create canvas panels
/// </summary>
[RequireComponent(typeof(Canvas))]
public class UICanvas : MonoBehaviour
{
    protected IUIManager manager;

    protected Canvas canvas;

    public virtual void Init(IUIManager uIManager)
    {
        manager = uIManager;
        canvas = GetComponent<Canvas>();
    }


    public virtual void SetActive(bool active)
    {
        canvas.enabled = active;
        gameObject.SetActive(active);
    }

}
