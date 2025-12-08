using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PopUpTextManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject display;
    public static PopUpTextManager instance;

    private readonly Queue<IEnumerator> queue = new Queue<IEnumerator>();
    private bool isRunning = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowPopUpText(string message, float duration)
    {
        Debug.Log($"Queueing pop-up text: {message} for {duration} seconds.");
        queue.Enqueue(ShowTextCoroutine(message, duration));

        if (!isRunning)
            StartCoroutine(RunQueue());
    }

    private IEnumerator ShowTextCoroutine(string message, float duration)
    {
        text.text = message;
        display.SetActive(true);

        yield return new WaitForSeconds(duration);

        text.text = "";
        display.SetActive(false);
    }

    private IEnumerator RunQueue()
    {
        isRunning = true;

        while (queue.Count > 0)
        {
            yield return StartCoroutine(queue.Dequeue());
        }

        isRunning = false;
    }

}
