using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour
{

    World world;
    public PlayerController player;

    public RectTransform highlight;
    public ItemSlot[] itemSlots;

    public int slotIndex = 0;
    public Text barText;

    private void Start()
    {

        world = GameObject.Find("World").GetComponent<World>();

        foreach (ItemSlot slot in itemSlots)
        {
            //slot.icon.sprite = world.blocks[slot.itemID].icon;
            slot.itemCount = -1;
            slot.icon.enabled = false;
            ActualiseBarText();
        }

    }

    public void addItem(int _itemCount, int itemIndex)
    {
        for (int i = 0; i < itemSlots.Length; i++) {
            ItemSlot slot = itemSlots[i];
            if (slot.itemID == itemIndex) {
            
                slot.itemCount += _itemCount;
                ActualiseBarText();

                return;
            }
        }
        for (int i = 0; i < itemSlots.Length; i++)
        {
            ItemSlot slot = itemSlots[i];
            if (slot.itemCount == -1) {

                slot.itemID = itemIndex;
                slot.icon.sprite = world.blocks[slot.itemID].icon;
                slot.itemCount = _itemCount;
                slot.icon.enabled = true;
                ActualiseBarText();

                return;
            }
        }
    }

    public void deleteItem(int _itemCount, int itemIndex)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            ItemSlot slot = itemSlots[i];
            if (slot.itemID == itemIndex)
            {
                if (slot.itemCount <= 1)
                {
                    slot.itemID = -1;
                    slot.itemCount = -1;
                    slot.icon.enabled = false;
                    ActualiseBarText();

                    return;
                }
                slot.itemCount -= _itemCount;
                ActualiseBarText();
                return;
            }
        }
    }

    void ActualiseBarText ()
    {
        barText.text = "";
        if (itemSlots[slotIndex].itemCount > 0)
        {
            barText.text += world.blocks[itemSlots[slotIndex].itemID].blockName;
            barText.text += " \t (×";
            barText.text += itemSlots[slotIndex].itemCount;
            barText.text += ")";
        }
    }

    private void Update()
    {

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {

            if (scroll > 0)
                slotIndex--;
            else
                slotIndex++;

            if (slotIndex > itemSlots.Length - 1)
                slotIndex = 0;
            if (slotIndex < 0)
                slotIndex = itemSlots.Length - 1;

            highlight.position = itemSlots[slotIndex].icon.transform.position;
            player.selectedBlockIndex = itemSlots[slotIndex].itemID;

            ActualiseBarText();

        }


    }


}

[System.Serializable]
public class ItemSlot
{

    public int itemID;
    public Image icon;
    public int itemCount;

}