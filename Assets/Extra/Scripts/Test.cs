using Oyedoyin.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    //SilantroInput SilantroInput = new SilantroInput();
    private SilantroControl silantroControl;

    [SerializeField] private SilantroCore silantroCore;

    private float testvalue;
    private float pitchvalue;
    private float rollvalue;
    private float rollpitch;

    private void Awake()
    {
        Debug.Log("Awake");
        silantroControl = new SilantroControl();
    }

    private void OnEnable()
    {
        silantroControl.Aircraft.Enable();
        //silantroControl.Aircraft.ThrottleLeverNew.started += ThrottleLeverNew;
        silantroControl.Aircraft.ThrottleLeverNew.performed += ThrottleLeverNew;
        ///silantroControl.Aircraft.ThrottleLeverNew.canceled += ThrottleLeverNew;

        silantroControl.Aircraft.PitchLeverNew.started += pitchLeverNew;
        silantroControl.Aircraft.PitchLeverNew.performed += pitchLeverNew;

        silantroControl.Aircraft.RollLeverNew.started += rollLeverNew;
        silantroControl.Aircraft.RollLeverNew.performed += rollLeverNew;

        //silantroControl.Aircraft.Turn.started += Turn;
        //silantroControl.Aircraft.Turn.performed += Turn;

    }

    private void OnDisable()
    {
        silantroControl.Aircraft.Disable();
        //silantroControl.Aircraft.ThrottleLeverNew.started -= ThrottleLeverNew;
        silantroControl.Aircraft.ThrottleLeverNew.performed -= ThrottleLeverNew;
        //silantroControl.Aircraft.ThrottleLeverNew.canceled -= ThrottleLeverNew;

        silantroControl.Aircraft.PitchLeverNew.started -= pitchLeverNew;
        silantroControl.Aircraft.PitchLeverNew.performed -= pitchLeverNew;

        silantroControl.Aircraft.RollLeverNew.started -= rollLeverNew;
        silantroControl.Aircraft.RollLeverNew.performed -= rollLeverNew;

        //silantroControl.Aircraft.Turn.started -= Turn;
        //silantroControl.Aircraft.Turn.performed -= Turn;
    }

    private void rollLeverNew(InputAction.CallbackContext context)
    {
        Debug.Log("rollLeverNew: " + context.ReadValue<float>());
        if (context.ReadValue<float>() > 0)
        {
            silantroCore.controller.m_input._rollInput += 0.01f;

            rollvalue += 0.01f;
        }
        else
        {
            silantroCore.controller.m_input._rollInput -= 0.01f;

            rollvalue -= 0.01f;
        }
    }

    private void pitchLeverNew(InputAction.CallbackContext context)
    {
        Debug.Log("pitchLeverNew: " + context.ReadValue<float>());
        if (context.ReadValue<float>() > 0)
        {
            silantroCore.controller.m_input._pitchInput += 0.01f;

            pitchvalue += 0.01f;
        }
        else
        {
            silantroCore.controller.m_input._pitchInput -= 0.01f;

            pitchvalue -= 0.01f;
        }
    }
   

    private void ThrottleLeverNew(InputAction.CallbackContext context)
    {
        Debug.Log("ThrottleLeverNew: " + context.ReadValue<float>());
        if (context.ReadValue<float>() > 0)
        {
            silantroCore.controller.m_input._throttleInput += 0.01f;
            
            testvalue += 0.01f;
        }
        else
        {
            silantroCore.controller.m_input._throttleInput -= 0.01f;
            
            testvalue -= 0.01f;
        }
    }

    private void Turn(InputAction.CallbackContext context)
    {
        Debug.Log("ThrottleLeverNew: " + context.ReadValue<float>());
        if (context.ReadValue<float>() > 0)
        {
            silantroCore.controller.m_input.keyboardRollInput += 0.01f;

            rollpitch += 0.01f;
        }
        else
        {
            silantroCore.controller.m_input.keyboardRollInput -= 0.01f;

            rollpitch -= 0.01f;
        }
    }
    
    private void Update()
    {
        //silantroCore.controller.m_input._pitchTrimInput = testvalue;
        //silantroCore.controller.m_input._rawPitchInput = pitchvalue;
        //silantroCore.controller.m_input._rawRollInput = testvalue;
        //silantroCore.controller.m_input._rawPitchInput = testvalue;
    }
}
