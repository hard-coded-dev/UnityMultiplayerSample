﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : Singleton<CanvasManager>
{
    public GameObject deadUI;

    public Toggle prediction;
    public Toggle reconciliation;
    public Toggle interpolation;
    public TMP_InputField lagTimeField;

    private void Awake()
    {
    }

    void Start()
    {
        interpolation.isOn = true;
    }

    public void OnLagChanged()
    {
        float newLag;
        if( float.TryParse( lagTimeField.text, out newLag ) )
        {
        }
    }
}
