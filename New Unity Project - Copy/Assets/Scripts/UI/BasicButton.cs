using UnityEngine;
using UnityEngine.EventSystems;

public class BasicButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioClip buttonHoverSound;
    private Vector3 originalScale;
    public float scaleMultiplier = 1.1f;


    void Awake()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonHoverSound)
        {
            AudioManager.Instance.Play(buttonHoverSound);
        }
        transform.localScale = originalScale * scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
