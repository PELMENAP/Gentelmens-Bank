using UnityEngine;

public class MultiDisplaySetup : MonoBehaviour 
{
    [SerializeField] private Camera operatorCam, viewerCam;
    private void Start() 
    {
        operatorCam.targetDisplay = 0;
        viewerCam.targetDisplay = 2;

        if (Display.displays.Length > 1) 
        {
            Display.displays[2].Activate();
        }
    }

    public void SetTheDisplay(int i)
    {
        viewerCam.targetDisplay = i;
        if (Display.displays.Length > 1) 
        {
            Display.displays[i].Activate();
        }
    }

    public void ApplicationQuit()
    {
        Application.Quit();
    } 
}