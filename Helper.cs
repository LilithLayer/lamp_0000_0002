using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public GameObject panel;

    public List<Cut> cuts = new List<Cut>();

    public bool inUI;

    public int cutID;

    public Missions missionsManager;

    void Start()
    {
        cutID = 0;
        CloseAll();
    }

    public void CallHelpByID(int memoryID)
    {
        cutID = memoryID;
        OpenHelper();
    }

    public void OpenHelper()
    {
        panel.SetActive(true);
        cuts[cutID].text.SetActive(true);
        inUI = true;
    }

    public void CloseHelper()
    {
        cuts[cutID].text.SetActive(false);
        panel.SetActive(false);
        inUI = false;
    }

    public void CloseAll()
    {
        for (int i = 0; i < cuts.Count; i++)
        {
            cuts[i].text.SetActive(false);
        }
        CloseHelper();
        inUI = false;
    }

    public void NextPage()
    {
        if (cuts[cutID].newMission)
        {
            missionsManager.missionID = cuts[cutID].missionID;
        }

        if (cuts[cutID].dialogueStopAfter)
        {
            CloseAll();
        }
        else
        {
            CloseAll();
            CallHelpByID(cutID + 1);
        }
    }
}

[System.Serializable]
public class Cut
{
    public GameObject text;

    public bool dialogueStopAfter;
    public bool newMission;

    [Header("(if mission)")]
    public int missionID;
}