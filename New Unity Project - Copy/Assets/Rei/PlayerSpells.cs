using System.Collections;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{

    public SpellObject currentSpell;

    private float currentChargeTime = 0.0f;
    private bool isCharging = false;
    GameObject spellInGame;
    public GameObject spawnLocation;


    private void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Mouse1) && currentSpell)
        {
            GetComponentInParent<PlayerController>().FreezeMidAir();
            // Instantiate the spell prefab at the player's position
            Vector2 spawnPoint = spawnLocation.transform.position; 
            spellInGame = Instantiate(currentSpell.BaseParticleEffect, spawnPoint, Quaternion.LookRotation(Vector3.up, Vector3.up));
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!isCharging)
            {
                isCharging = true;
                currentChargeTime = 0.0f; // Start or reset the charge timer              
            }

            currentChargeTime += Time.deltaTime; // Increment the charge timer


            // Check if the required charge time has been met
            if (currentChargeTime >= currentSpell.chargeTime)
            {
                // Perform the charged ability action here
                CastCurrentSpell();
                GetComponentInParent<PlayerController>().UnfreezeMidAir();
                isCharging = false; // Reset for next charge
                currentChargeTime = 0.0f;
            }
        }
        else
        {
            // If the player releases the button before the charge time is met, reset the charge
            if (isCharging)
            {
                isCharging = false; // Reset charging state
                currentChargeTime = 0.0f;             
            }
        }
        // Check if the charge button was released
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
                GetComponentInParent<PlayerController>().UnfreezeMidAir();
                Destroy(spellInGame);
                if (isCharging)
                 {
                isCharging = false; // Reset charging state
                currentChargeTime = 0.0f;             
                }
        }
    }
    

    public void CastCurrentSpell()
    {
        if (currentSpell)
        {
            //currentSpell.Cast();
            Vector3 mouseScreenPos = Input.mousePosition;
            // For 2D or 3D on a fixed plane (e.g., XZ plane for a top-down game)
            // Set the Z-component of mouseScreenPos to the desired world Z-coordinate (or player's Z)
            mouseScreenPos.z = transform.position.z;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 spawnPoint =  transform.position; 
            Vector2 direction = (mouseWorldPos - spawnPoint).normalized;
           // StartCoroutine(FreezeMidAirCoroutine(0.5f));
            spellInGame.GetComponent<Rigidbody2D>().linearVelocity = direction * currentSpell.spellSpeed; 
            GetComponent<Animator>().SetTrigger("CastSpell");
        }
    }

    public void SetSpell(SpellObject newSpell)
    {
        currentSpell = newSpell;
    }

    public IEnumerator FreezeMidAirCoroutine(float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
    }
}
