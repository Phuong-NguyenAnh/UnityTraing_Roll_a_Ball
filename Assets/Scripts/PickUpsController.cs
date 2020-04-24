using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpsController : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject pickUpPrefab;

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
                pickUp.transform.parent = gameObject.transform;
                // Do something with the object that was hit by the raycast.
            }
        }
    }
}