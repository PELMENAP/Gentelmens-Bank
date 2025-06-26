using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Inventory/InventoryData")]
public class InventoryData : ScriptableObject {
    [Tooltip("Просто задавайте нужное число слотов через инспектор")]
    public List<InventorySlot> slots = new List<InventorySlot>();
}