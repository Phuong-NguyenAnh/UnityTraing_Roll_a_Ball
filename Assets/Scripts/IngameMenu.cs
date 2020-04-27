using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    public UnityEngine.Events.UnityAction backBtnPressedCallback = null;
    public void BackBtnPressed()
    {
        gameObject.SetActive(false);
        if (backBtnPressedCallback != null) backBtnPressedCallback();
    }
}
