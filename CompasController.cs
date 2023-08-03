using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompasController : MonoBehaviour
{
    private Transform target;
    private bool activated;

    public Transform player;
    public Vector3 offset;
    public GameObject gfx;

    void Start()
    {
        CloseCompas();
    }

    void Update ()
    {
        transform.position = player.position + offset;
    }

    void FixedUpdate()
    {
        if (activated)
        {
            transform.LookAt(target.position);
        }
    }

    public void OpenCompas(Transform _target)
    {
        activated = true;
        target = _target;
        gfx.SetActive(true);
    }

    public void CloseCompas()
    {
        activated = false;
        gfx.SetActive(false);
    }
}
