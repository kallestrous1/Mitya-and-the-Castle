using System.Collections;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{

    public SpellObject currentSpell;

    public float minScale = 0.1f;       // starting size
    public float maxScale = 0.6f;       // size when fully charged
    public float lifeTime = 3.0f;          // lifetime of the spell after being cast
    private float currentChargeTime = 0.0f;
    private bool isCharging = false;
    GameObject spellInGameParticle;
    public GameObject spawnLocation;
    bool spellFullyCharged = false;

    private void Update()
    {
        // Start charging (instantiate at key down)
        if (Input.GetKeyDown(KeyCode.Mouse1) && currentSpell)
        {
            // instantiate at spawnLocation position
            Vector3 spawnPoint = spawnLocation ? spawnLocation.transform.position : transform.position;

            if (currentSpell.BaseParticleEffect)
            {
                spellInGameParticle = Instantiate(currentSpell.BaseParticleEffect, spawnPoint, Quaternion.identity);
                // force starting scale to minScale
                spellInGameParticle.transform.localScale = new Vector3(minScale, minScale, 1f);

                Rigidbody2D rb = spellInGameParticle.GetComponent<Rigidbody2D>();
                if (rb) rb.gravityScale = 0f;
            }

            // begin charging state
            GetComponentInParent<PlayerController>().FreezeMidAir();
            isCharging = true;
            spellFullyCharged = false;
            currentChargeTime = 0f;

            // optional: freeze player mid-air if you want
            // GetComponentInParent<PlayerController>().FreezeMidAir();
        }

        // While holding the button -> charge + scale
        if (Input.GetKey(KeyCode.Mouse1) && isCharging && currentSpell)
        {
            currentChargeTime += Time.deltaTime;

            // Scale spell while charging
            float t = Mathf.Clamp01(currentChargeTime / currentSpell.chargeTime);
            float scale = Mathf.Lerp(minScale, maxScale, t);
            if (spellInGameParticle)
            {
                spellInGameParticle.transform.localScale = new Vector3(scale, scale, 1f);
            }

            // Fully charged: cast the spell
            if (currentChargeTime >= currentSpell.chargeTime)
            {
                spellFullyCharged = true;
                CastCurrentSpell();

                // mark charging finished
                GetComponentInParent<PlayerController>().UnfreezeMidAir();
                isCharging = false;
                currentChargeTime = 0f;
            }
        }

        // Button released (key up) handling
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            // Unfreeze player if you froze them
            GetComponentInParent<PlayerController>().UnfreezeMidAir();

            // If we were charging and didn't reach full charge -> cancel & destroy the orb
            if (!spellFullyCharged && spellInGameParticle)
            {
                Destroy(spellInGameParticle);
                spellInGameParticle = null;
            }

            // reset charging flags
            isCharging = false;
            currentChargeTime = 0f;
            spellFullyCharged = false;
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
            Vector2 targetDir = ((Vector2)mouseWorldPos - spawnPoint).normalized;

            // Add arc by boosting vertical aim a little
            targetDir.y += 0.35f; // tweak between 0–0.6
            Vector2 direction = targetDir.normalized;
            // StartCoroutine(FreezeMidAirCoroutine(0.5f));
            Rigidbody2D rb = spellInGameParticle.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.gravityScale = 1.5f; // whatever gravity you want for throw
                rb.linearVelocity = Vector2.zero; // reset in case it's floating with scale animations
                rb.AddForce(direction * currentSpell.spellSpeed);
            }
            // spellInGameParticle.GetComponent<Rigidbody2D>().linearVelocity = direction * currentSpell.spellSpeed;         
            //spellInGameParticle.GetComponent<Rigidbody2D>().AddForce(direction * currentSpell.spellSpeed, ForceMode2D.Impulse);
            GetComponent<Animator>().SetTrigger("CastSpell");
            GetComponentInParent<PlayerController>().UnfreezeMidAir();
            Destroy(spellInGameParticle, lifeTime);
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
