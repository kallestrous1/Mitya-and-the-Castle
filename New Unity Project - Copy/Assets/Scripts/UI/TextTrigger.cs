using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    public string message;
    public float displayDuration = 2f;
    public bool triggerOnce = true;
    public bool toBeTriggered = false;

    public StateChangingObject stateChangingObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && toBeTriggered)
        {   
            if (PopUpTextManager.instance != null)
            {
                PopUpTextManager.instance.ShowPopUpText(message, displayDuration);
                if (triggerOnce)
                {
                    toBeTriggered = false;
                    stateChangingObject.ChangeObjectState(false);
                }
            }
        }
    }

}
