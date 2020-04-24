using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public Text countText;
    public Text winText;
    public GameObject pickUps;
    public Animator anim;

    public Rigidbody rb;

    private int count;

    private void Start()
    {
        count = 0;
        SetCountText();
        winText.text = "";
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        rb.AddForce(movement * speed);

        Transform target = null;
        for (int i = 0; i < pickUps.transform.childCount; i++)
        {
            if (target == null)
            {
                var tempTarget = pickUps.transform.GetChild(i);
                var script = tempTarget.GetComponent<PickUp>();
                if (!script.isDie)
                {
                    target = tempTarget;
                }
            }
            else
            {
                break;
            }
        }

        if (target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            var script = other.GetComponent<PickUp>();
            if (!script.isDie)
            {
                script.Die();
                count++;
                SetCountText();
            }
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    private void LateUpdate()
    {
        if (pickUps.transform.childCount == 0)
        {
            anim.SetTrigger("ToIdle");
        }
    }
}