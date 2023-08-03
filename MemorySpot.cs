using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemorySpot : MonoBehaviour
{

    public float callingDistance;
    public Vector3 mgPos;
    public int helpID;
    public Helper helper;

    public Transform mg;
    public Transform p;
    private bool declared;

    // Start is called before the first frame update
    void Start()
    {
        declared = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!declared && Vector2.Distance(transform.position, p.position) < callingDistance)
        {
            mg.gameObject.GetComponent<GuardianController>().targetPos = mgPos;
            declared = true;
            helper.CallHelpByID(helpID);
        }
        else
        {
            if (declared)
            {
                Destroy(gameObject);
            }
        }
    }
}
