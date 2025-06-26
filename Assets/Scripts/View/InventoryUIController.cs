using UnityEngine;
using System.Collections.Generic;

public class InventoryUIController : MonoBehaviour 
{
    [Header("Привяжите в инспекторе")]
    public InventoryData inventoryData;
    public RectTransform slotsContainer;
    public RectTransform itemsContainer;
    [SerializeField] private bool saveSlotData;
    public SimpleShader[] simpleShaders;

    [HideInInspector] public List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();
    [HideInInspector] public List<ItemButtonUI> itemButtonUIs = new List<ItemButtonUI>();
    [HideInInspector] public InventorySlotUI selectedSlot;

    private void Start() 
    {
        slotUIs.Clear();
        var slotComponents = slotsContainer.GetComponentsInChildren<InventorySlotUI>(includeInactive: false);
        for (int i = 0; i < slotComponents.Length; i++) 
        {
            var ui = slotComponents[i];
            ui.Init(i, this, saveSlotData ? inventoryData.slots[i] : null);
            slotUIs.Add(ui);
        }

        itemButtonUIs.Clear();
        var itemComponents = itemsContainer.GetComponentsInChildren<ItemButtonUI>(includeInactive: false);
        foreach (var ui in itemComponents) 
        {
            ui.Init(this);
            itemButtonUIs.Add(ui);
        }
    }
    public void ClearAllSlots()
    {
        foreach (var slot in slotUIs)
        {
            slot.AssignItem(null);
        }
        for (int i = 0; i < simpleShaders.Length; i++)
        {
            simpleShaders[i].ClearHighligh();
        }
    }

    public void SelectSlot(InventorySlotUI slot) 
    {
        if (selectedSlot != null)
            selectedSlot.SetHighlight(false);

        selectedSlot = slot;
        selectedSlot.SetHighlight(true);
    }

    public void OnItemClicked(Item item) 
    {
        if (selectedSlot == null) return;
        selectedSlot.AssignItem(item);
    }
}