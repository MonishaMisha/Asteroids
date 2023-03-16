using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Use as controller touch buttons for mobile
/// </summary>
public class MobileControllerButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    [SerializeField]
    InputAction actionType;

    public void OnPointerDown(PointerEventData eventData)
    {
        InputController.SendMobileInput(actionType, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputController.SendMobileInput(actionType, false);
    }
}
