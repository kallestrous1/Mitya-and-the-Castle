using UnityEngine;

public class VolumeButton : BasicButton
{

    public GameObject volumeMenu;
    public GameObject controlsMenu;

    public override void OnClicked()
    {
        base.OnClicked();
        if (!volumeMenu.activeSelf)
        {
            controlsMenu.SetActive(false);
            volumeMenu.SetActive(true);
        }
        else
        {
            volumeMenu.SetActive(false);
        }

    }
}
