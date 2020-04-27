using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public bool isDie = false;
    public bool isRed = false;
    public Renderer mRenderer;
    public Animator mAnimator;

    public GameObject firePrefab;
    public GameObject explosionPrefab;

    public int redDamage = 2;

    private void Start()
    {
        var random = new System.Random();
        int color = random.Next(1, 4);
        mRenderer.material.color = new Color(color % 2, color % 3, 0, 1);
        if (color == 3)
        {
            var fire = Instantiate(firePrefab);
            fire.transform.parent = gameObject.transform;
            fire.transform.position = transform.position;
            isRed = true;
        }
    }

    public void Die()
    {
        isDie = true;
        mAnimator.SetTrigger("Die");
        if (isRed)
        {
            var explosion = Instantiate(explosionPrefab);
            explosion.transform.position = transform.position;
        }
    }

    public void DieFinish()
    {
        Destroy(gameObject);
    }
}