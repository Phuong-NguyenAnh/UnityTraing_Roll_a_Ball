using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public GameObject optionMenu;
    public GameObject stateIngame;

    void OnEnable()
    {
        stateIngame.SetActive(false);
    }

    public void PlayPressed()
    {
        gameObject.SetActive(false);
        stateIngame.SetActive(true);
    }
    public void OptionPressed()
    {
        gameObject.SetActive(false);
        optionMenu.SetActive(true);
        optionMenu.GetComponent<BuildingMenu>().onBackPressCallback = () => gameObject.SetActive(true);
    }
    public void ExitPressed()
    {

    }
}
