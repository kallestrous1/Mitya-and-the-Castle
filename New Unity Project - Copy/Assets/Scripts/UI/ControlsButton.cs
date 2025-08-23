using UnityEngine;

public class ControlsButton : BasicButton
{

    public GameObject volumeMenu;
    public GameObject controlsMenu;

    public override void OnClicked()
    {
        base.OnClicked();
        if (!controlsMenu.activeSelf)
        {
            volumeMenu.SetActive(false);
            controlsMenu.SetActive(true);
        }
        else
        {
            controlsMenu.SetActive(false);
        }

    }
}

