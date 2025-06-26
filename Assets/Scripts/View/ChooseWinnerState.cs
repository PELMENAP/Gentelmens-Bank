using UnityEngine;
public class ChooseWinnerState : IVisualState
{
    private GameObject component, visual;

    public ChooseWinnerState(GameObject component, GameObject visual)
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