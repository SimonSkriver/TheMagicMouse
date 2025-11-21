// PlayerInventory.cs
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryItem heldItem;

    public bool HasItem(InventoryItem item)
    {
        return heldItem == item;
    }

    public void PickUpItem(InventoryItem item)
    {
        heldItem = item;
    }
}