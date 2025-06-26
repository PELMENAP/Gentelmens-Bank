using UnityEngine;

public class MainGameState : IVisualState
{
    private GameObject component, visual;

    public MainGameState(GameObject component, GameObject visual)
    {
        this.component = component;
        this.visual = visual;
    }

    public void EnterState()
    {
        component?.SetActive(false);
        visual.transform.SetAsLastSibling();
    }

    public void ExitState()
    {
        component?.SetActive(true);
    }
}
