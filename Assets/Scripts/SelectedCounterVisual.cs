using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] private _BaseCounter clearCounter;
    [SerializeField] private GameObject selectedCounterVisual;

    private void Start()
    {
        PlayerController.Instance.OnSelectedCounterChanged += PlayerController_OnSelectedCounterChanged;
    }

    private void PlayerController_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == clearCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        selectedCounterVisual.SetActive(true);
    }

    private void Hide()
    {
        selectedCounterVisual.SetActive(false);
    }
}
