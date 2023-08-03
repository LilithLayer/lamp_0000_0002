using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public GameObject inventory;
    public PlayerController playerController;

    void Start ()
    {
        QuitInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            if (playerController.inInventory)
            {
                QuitInventory();
            }
            else
            {
                EnterInventory();
            }
        }
    }

    void EnterInventory()
    {
        playerController.inInventory = true;
        inventory.SetActive(true);
    }

    void QuitInventory ()
    {
        playerController.inInventory = false;
        inventory.SetActive(false);
    }
}
