using System.Collections;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{

    public SpellObject currentSpell;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            CastCurrentSpell();
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
            GameObject spell = Instantiate(currentSpell.BaseParticleEffect, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
            StartCoroutine(FreezeMidAirCoroutine(0.5f));
            spell.GetComponent<Rigidbody2D>().linearVelocity = direction * currentSpell.spellSpeed; 
            GetComponent<Animator>().SetTrigger("CastSpell");
        }
    }

    public void SetSpell(SpellObject newSpell)
    {
        currentSpell = newSpell;
    }

    public IEnumerator FreezeMidAirCoroutine(float freezeTime)
    {
        GetComponentInParent<PlayerController>().FreezeMidAir();
        yield return new WaitForSeconds(freezeTime);
        GetComponentInParent<PlayerController>().UnfreezeMidAir();
    }
}
