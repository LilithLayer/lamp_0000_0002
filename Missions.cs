using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour
{
    [Header("General")]
    [Header("")]
    public Transform player;
    public Transform memoryGuardian;
    public Helper helper;
    public CompasController compas;
    [Header("Infos")]
    public int missionID;

    [Header("")]
    [Header("Mission 0001 : myrandoles")]
    public Transform myrandoles;
    public int endMissionHelperTextID;
    [Header("Infos")]
    public bool myrandolesTaken;
    public bool hasReturnedToMG;

    public void Start ()
    {
        missionID = 0;

        InitMission0001();
    }

    void Update ()
    {
        if (missionID == 1) { Mission0001(); }
    }

    void InitMission0001()
    {
        myrandolesTaken = false;
        hasReturnedToMG = false;
        myrandoles.gameObject.SetActive(true);
    }

    public void Mission0001 ()
    {
        if (!myrandolesTaken)
        {
            compas.OpenCompas(myrandoles);
            if (Vector2.Distance(player.position, myrandoles.position) < 1f)
            {
                compas.CloseCompas();
                myrandolesTaken = true;
                myrandoles.gameObject.SetActive(false);
            }
        }
        else
        {
            if (!hasReturnedToMG)
            {
                compas.OpenCompas(memoryGuardian);
                if (Vector2.Distance(player.position, memoryGuardian.position) < 3f)
                {
                    compas.CloseCompas();
                    hasReturnedToMG = true;
                }
            }
            else
            {
                EndMission0001();
            }
        }
    }

    void EndMission0001()
    {
        helper.CallHelpByID(endMissionHelperTextID);
        missionID = 0;
    }
}
