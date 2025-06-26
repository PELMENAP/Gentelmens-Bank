using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject gayObject, prizes;
    private void Start() 
    { 
        Screen.fullScreen = true;
        Screen.SetResolution(1920, 1080, true); 
    } 

    public void ToggleFullScreen() 
    { 
        Screen.fullScreen = !Screen.fullScreen; 
    } 
    public void SetSettingsAble()
    {
        gayObject.SetActive(!gayObject.activeSelf);
    }

    public void SetPrizesAble()
    {
        prizes.SetActive(!prizes.activeSelf);
    }
}
