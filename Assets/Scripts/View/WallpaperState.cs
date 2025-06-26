using UnityEngine;

public class WallpaperState : IVisualState
{
    private GameObject component, visual;

    public WallpaperState(GameObject component, GameObject visual)
    {
        this.component = component;
        this.visual = visual;
    }

    public void EnterState()
    {
        if(component != null) component.SetActive(false);
        visual.transform.SetAsLastSibling();
    }

    public void ExitState()
    {
        if(component != null) component.SetActive(true);
    }
}