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

    [SerializeField] public Animator playerAni;

    #region Input Handling

    private void Start()
    {

    }
    public void OnEnable()
    {
        InputManager.Instance.SpellPressed += OnSpellPressed;
        InputManager.Instance.SpellReleased += OnSpellReleased;
    }
    public void OnDisable()
    {
        InputManager.Instance.SpellPressed -= OnSpellPressed;
        InputManager.Instance.SpellReleased -= OnSpellReleased;
    }

    private void OnSpellPressed()
    {
        StartCharging();
    }

    private void OnSpellReleased()
    {
        ReleaseSpell();
    }

    #endregion

    private void Update()
    {
        if (!isCharging) return;

        currentChargeTime += Time.deltaTime;

        float t = Mathf.Clamp01(currentChargeTime / currentSpell.chargeTime);
        float scale = Mathf.Lerp(minScale, maxScale, t);

        if (spellInGameParticle)
            spellInGameParticle.transform.localScale = new Vector3(scale, scale, 1f);

        if (currentChargeTime >= currentSpell.chargeTime && !spellFullyCharged)
        {
            spellFullyCharged = true;
            CastCurrentSpell();
            StopCharging();
        }
    }
    private void StartCharging()
    {
        if (!currentSpell) return;

        CreateSpell();

        isCharging = true;
        currentChargeTime = 0f;
        spellFullyCharged = false;

        GetComponentInParent<PlayerController>().FreezeMidAir();
    }

    private void ReleaseSpell()
    {
        if (!isCharging) return;

        if (!spellFullyCharged)
        {
            // cancelled early -> destroy orb
            Destroy(spellInGameParticle);
        }
        else
        {
            // if fully charged, spell was cast already in Update
        }
        StopCharging();
    }

    private void StopCharging()
    {
        playerAni.SetTrigger("Stopcast");
        isCharging = false;
        currentChargeTime = 0f;
        spellFullyCharged = false;

        GetComponentInParent<PlayerController>().UnfreezeMidAir();
    }


    private void CreateSpell()
    {
        Vector3 spawnPoint = spawnLocation ? spawnLocation.transform.position : transform.position;

        playerAni.SetTrigger("Attack");
        if (currentSpell.BaseParticleEffect)
        {
            spellInGameParticle = Instantiate(currentSpell.BaseParticleEffect, spawnPoint, Quaternion.identity);

            spellInGameParticle.transform.SetParent(spawnLocation.transform, worldPositionStays: false);

            spellInGameParticle.transform.localPosition = Vector3.zero;
            spellInGameParticle.transform.localRotation = Quaternion.identity;
            spellInGameParticle.transform.localScale = Vector3.one * minScale;

            // force starting scale to minScale

            Rigidbody2D rb = spellInGameParticle.GetComponent<Rigidbody2D>();
            if (rb) rb.gravityScale = 0f;
        }

        // begin charging state
        GetComponentInParent<PlayerController>().FreezeMidAir();
        isCharging = true;
        spellFullyCharged = false;
        currentChargeTime = 0f;
    }

    public void CastCurrentSpell()
    {
        if (!currentSpell) return;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direction = (mouseWorldPos - (Vector2)spawnLocation.transform.position);

            // Add arc by boosting vertical aim a little
            direction.y += 0.35f; // tweak between 0–0.6
            direction = direction.normalized;

         spellInGameParticle.transform.SetParent(null, worldPositionStays: true);

        // StartCoroutine(FreezeMidAirCoroutine(0.5f));
        Rigidbody2D rb = spellInGameParticle.GetComponent<Rigidbody2D>();
            if (rb)
            {
                spellInGameParticle.transform.SetParent(null, worldPositionStays: true);
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

    public void SetSpell(SpellObject newSpell)
    {
        currentSpell = newSpell;
    }

    public IEnumerator FreezeMidAirCoroutine(float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
    }
}
