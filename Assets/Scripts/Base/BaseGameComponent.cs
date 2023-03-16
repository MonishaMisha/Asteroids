using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Component class which has the scenemanager dependency and default initialization
/// </summary>
public class BaseGameComponent : MonoBehaviour, IBaseGameComponent
{
    [Tooltip("Check this, if child object with BaseGameComponent to get initialized"), SerializeField]
    bool checkForDependency = false;
    [Tooltip("Check this, if this component needs to be referenced in the GameComponentManager"), SerializeField]
    bool addReference = false;

    protected IGameComponentManager _sceneManager;

    public virtual void Init(IGameComponentManager scenemanager)
    {
        _sceneManager = scenemanager;

        if (checkForDependency)
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out BaseGameComponent component))
                {
                    component.Init(_sceneManager);
                }
            }
        }
        if (addReference)
        {
            _sceneManager.SetReference(this);
        }
        Debug.Log("Base Game Component of type " + this.GetType()+"is initialized");
    }
}

public interface IBaseGameComponent
{
    void Init(IGameComponentManager scenemanager);
}