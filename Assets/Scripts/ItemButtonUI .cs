using UnityEngine;
using UnityEngine.UI;
public class ItemButtonUI : MonoBehaviour 
{
    public Item item;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    private InventoryUIController controller;

    public void Init(InventoryUIController ctrl) 
    {
        controller = ctrl;
        if(item != null)
            image.sprite = item.icon;
        button.onClick.AddListener(() => controller.OnItemClicked(item));
    }
}
