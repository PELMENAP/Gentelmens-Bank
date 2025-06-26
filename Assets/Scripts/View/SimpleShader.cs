using UnityEngine;
using UnityEngine.UI;
public class SimpleShader : MonoBehaviour
{
    [SerializeField] private Toggle[] toggle;
    [SerializeField] private Image[] images;
    [SerializeField] private Material defmaterial, highlightedMaterial;
    bool isHighligihet;
    public void ClearHighligh()
    {
        foreach (var item in images)
        {
            item.material = defmaterial;   
        }
        foreach (var item in toggle)
        {
            item.isOn = false;
        }
    }

    public void High()
    {
        for (int i = 0; i < toggle.Length; i++)
        {
            images[i].material = toggle[i].isOn ? highlightedMaterial : defmaterial;
        }
    }
}
