using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuildingMenu : MonoBehaviour
{

    public Button backBtn;

    public UnityAction onBackPressCallback = null;
    public void BackButtonPressed()
    {
        gameObject.SetActive(false);
        if (onBackPressCallback != null)
        {
            onBackPressCallback();
        }
    }
}
