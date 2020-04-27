using System;
using UnityEngine;
using UnityEngine.UI;

public class StateIngame : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject pickUpPrefab;
    public GameObject pickUps;
    public GameObject IngameMenu;

    public Player playerScript;

    public Text timerDisplay;

    private double playedTime = 0;

    void OnEnable()
    {
        timerDisplay.text = "00:00";
        playedTime = 0;

        foreach (Transform pickup in pickUps.transform)
        {
            Destroy(pickup.gameObject);
        }
        playerScript.Init();
    }

    void Update()
    {
        if (playerScript.IsPlaying())
        {
            playedTime += Time.deltaTime;
            timerDisplay.text = TimeSpan.FromSeconds(playedTime).ToString(@"mm\:ss");
        }

        if (playerScript.IsAlive())
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 point = hit.point;
                    point.y = 0.5f;
                    var pickUp = Instantiate(pickUpPrefab, point, Quaternion.identity);
                    pickUp.transform.parent = pickUps.transform;
                }
            }
    }

    public void PausePressed()
    {
        playerScript.SetPlayerState(Player.PlayerState.Pause);
        IngameMenu.SetActive(true);
        IngameMenu.GetComponent<IngameMenu>().backBtnPressedCallback = () => playerScript.resume();
    }
}
