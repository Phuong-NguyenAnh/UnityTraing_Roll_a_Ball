using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIngame : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject pickUpPrefab;
    public GameObject pickUps;
    public GameObject IngameMenu;

    public MAWPlayer playerScript;

    void Update()
    {
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
        playerScript.SetPlayerState(MAWPlayer.PlayerState.Pause);
        IngameMenu.SetActive(true);
        IngameMenu.GetComponent<IngameMenu>().backBtnPressedCallback = () => playerScript.resume();
    }
}
