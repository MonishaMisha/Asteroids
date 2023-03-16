using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum InputAction
{
    Fire,
    Thrust,
    Left_Steering,
    Right_Steering
}

/// <summary>
/// Used to pass the input feedbacks to the game
/// </summary>
public class InputController : BaseGameComponent
{
    [SerializeField]
    float sensitivityConstant;

    public delegate void onSteerInputEvent(bool isPerformed);
    public static event onSteerInputEvent OnSteerInputEvent;

    public delegate void onThrustInputEvent(bool isPerformed);
    public static event onThrustInputEvent OnThrustInputEvent;

    public delegate void onFireInputEvent(bool isPerformed);
    public static event onFireInputEvent OnFireInputEvent;


    public static float ThrustSensitivity;
    public static float SteerSensitivity;


    static bool isFiringAction = false;
    static bool isThrustAction = false;
    static bool isSteeringAction_L = false;
    static bool isSteeringAction_R = false;

    private void OnEnable()
    {
        GameManager.OnGameReset += OnGameReset;
    }
    private void OnDisable()
    {
        GameManager.OnGameReset -= OnGameReset;
    }

    private void OnGameReset()
    {
        isFiringAction = false;
        isThrustAction = false;
        isSteeringAction_L = false;
        isSteeringAction_R = false;

        ThrustSensitivity = 0;
        SteerSensitivity = 0;
    }

    /// <summary>
    /// Used to send touch(button) feedback to game
    /// </summary>
    /// <param name="Action"></param>
    /// <param name="performState"></param>
    public static void SendMobileInput(InputAction Action, bool performState)
    {
        switch (Action)
        {
            case InputAction.Fire:
                isFiringAction = performState;
                OnFireInputEvent?.Invoke(isFiringAction);
                break;

            case InputAction.Thrust:
                isThrustAction = performState;
                OnThrustInputEvent?.Invoke(isThrustAction);
                break;

            case InputAction.Left_Steering:
                isSteeringAction_L = performState;
                OnSteerInputEvent?.Invoke(isSteeringAction_L);
                break;

            case InputAction.Right_Steering:
                isSteeringAction_R = performState;
                OnSteerInputEvent?.Invoke(isSteeringAction_R);
                break;


        }
    }

    // Update is called once per frame
    void Update()
    {

        //Get keyboard inputs and send it as event to game

#if (UNITY_EDITOR || UNITY_STANDALONE)


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {

            if (!isFiringAction)
            {
                isFiringAction = true;
                OnFireInputEvent?.Invoke(isFiringAction);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            if (isFiringAction)
            {
                isFiringAction = false;
                OnFireInputEvent?.Invoke(isFiringAction);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!isSteeringAction_L)
            {
                isSteeringAction_L = true;
                OnSteerInputEvent?.Invoke(isSteeringAction_L);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (isSteeringAction_L)
            {
                isSteeringAction_L = false;
                OnSteerInputEvent?.Invoke(isSteeringAction_L);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!isSteeringAction_R)
            {
                isSteeringAction_R = true;
                OnSteerInputEvent?.Invoke(isSteeringAction_R);
            }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (isSteeringAction_R)
            {
                isSteeringAction_R = false;
                OnSteerInputEvent?.Invoke(isSteeringAction_R);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isThrustAction)
            {
                isThrustAction = true;
                OnThrustInputEvent?.Invoke(isThrustAction);
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (isThrustAction)
            {
                isThrustAction = false;
                OnThrustInputEvent?.Invoke(isThrustAction);
            }
        }



#endif
        if (isThrustAction)
        {
            ThrustSensitivity = Mathf.MoveTowards(ThrustSensitivity, 1f, Time.deltaTime * sensitivityConstant);
        }
        else
        {
            ThrustSensitivity = Mathf.MoveTowards(ThrustSensitivity, 0f, Time.deltaTime * sensitivityConstant);
        }


        if (isSteeringAction_L)
        {
            SteerSensitivity = Mathf.MoveTowards(SteerSensitivity, 1f, Time.deltaTime * sensitivityConstant);
        }
        else if (isSteeringAction_R)
        {
            SteerSensitivity = Mathf.MoveTowards(SteerSensitivity, -1f, Time.deltaTime * sensitivityConstant);
        }
        else
        {
            SteerSensitivity = Mathf.MoveTowards(SteerSensitivity, 0f, Time.deltaTime * sensitivityConstant);
        }

    }
}
