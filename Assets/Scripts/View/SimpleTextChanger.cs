using UnityEngine;
using TMPro;
public class SimpleTextChanger : MonoBehaviour
{
    private void Start() {
        InvokeRepeating("SynctTheText", 2.0f, 0.3f);
    }
    [SerializeField] private TextMeshProUGUI sourceText, textToSet;
    public void SynctTheText()
    {
        if(textToSet != null)
            textToSet.text = sourceText.text;
    }
}

