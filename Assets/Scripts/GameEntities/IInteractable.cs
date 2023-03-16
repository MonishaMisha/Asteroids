using System;

public interface IInteractable<T>
{
    void RegisterInteractionCallback(Action<T> callback);
}