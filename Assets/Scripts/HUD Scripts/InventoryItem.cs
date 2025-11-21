// InventoryItem.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Quest/Inventory Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon;
}