using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    public bool destroyOnEnd = true;
    private ParticleSystem ps;

    ////////////////////////////////////////////////////////////////

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive())
            {
                if (destroyOnEnd) Destroy(gameObject);
            }
        }
    }
}