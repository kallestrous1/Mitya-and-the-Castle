using System.Collections;
using UnityEngine;

public class RingPedestal : MonoBehaviour
{
    bool ringRequest;

    public AudioClip ringSound;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            ringRequest = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            if (ringRequest)
            {
                ringRequest = false;
                if (DialogueManager.instance.dialogueVariables.inkVariables["hasRing"].ToString().Equals("true"))
                {
                    StartCoroutine(ProcessRingRequest());                
                }
                 

            }
        }
    }


    private IEnumerator ProcessRingRequest()
    {
        this.GetComponentInChildren<ParticleSystem>().Simulate(0.0f, true, true);
        this.GetComponentInChildren<ParticleSystem>().Play();
        AudioManager.Instance.Play(ringSound);

        yield return new WaitForSeconds(5f);
        NewManager.manager.TriggerEndGame();
    }
}
