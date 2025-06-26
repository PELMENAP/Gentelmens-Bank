using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour 
{
    public Image icon;
    public Outline highlight;

    [SerializeField] private Button button;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private VisualSlotUI visualSlotUI;
    private int index;
    private InventoryUIController controller;

    public void Init(int idx, InventoryUIController ctrl, InventorySlot data) 
    {
        index = idx;
        controller = ctrl;
        button.onClick.AddListener(() => controller.SelectSlot(this));
        UpdateVisual(data);
        SetHighlight(false);
    }

    public void UpdateVisual(InventorySlot data) 
    {
        if (data != null && data.item != null) 
        {
            icon.sprite = data.item.icon;
            icon.enabled = true;
            visualSlotUI?.ChangeTheViewSprite(data.item.cardSprite);
        }
        else
        {
            icon.sprite = defaultSprite;
            visualSlotUI?.ChangeTheViewSprite();
        }
    }

    public void AssignItem(Item item) 
    {
        controller.inventoryData.slots[index].item = item;
        UpdateVisual(controller.inventoryData.slots[index]);
    }

    public void SetHighlight(bool on) 
    {
        highlight.enabled = on;
    }
}