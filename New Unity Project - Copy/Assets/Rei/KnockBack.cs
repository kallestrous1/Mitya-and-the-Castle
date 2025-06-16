using System.Collections;
using UnityEngine;

public class KnockBack : MonoBehaviour
{

    public float knockbackTime = 0.4f;
    public float hitDirectionForce = 25f;
    public float constForce = 10f;
    public float inputForce = 7.5f;

    public AnimationCurve knockbackForceCurve;

    private Rigidbody2D rb;
    private Collider2D playerCol;

    private Coroutine knockbackCoroutine;

    public bool isBeingKnockedBack { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();
    }

    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        isBeingKnockedBack = true;
        Physics2D.IgnoreLayerCollision(9, 10, true);

        Vector2 _hitForce;
        Vector2 _constantForce;
        Vector2 _knockbackForce;
        Vector2 _combinedForce;
        float _time = 0f;

        _constantForce = constantForceDirection * constForce;

        float _elapsedTime = 0f;
        while(_elapsedTime < knockbackTime)
        {
            _elapsedTime += Time.fixedDeltaTime;
            _time += Time.fixedDeltaTime;

            _hitForce = hitDirection * hitDirectionForce;

            _knockbackForce = _hitForce + _constantForce;

            if(inputDirection != 0)
            {
                _combinedForce = _knockbackForce + new Vector2(inputDirection * inputForce, 0f);
            }
            else
            {
                _combinedForce = _knockbackForce * knockbackForceCurve.Evaluate(_time);
            }

            rb.AddForce(_combinedForce, ForceMode2D.Impulse);

            yield return new WaitForFixedUpdate();
        }
        Physics2D.IgnoreLayerCollision(9, 10, false);

        isBeingKnockedBack = false;
    }

    public void StartPlayerKnockback(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        knockbackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
    }


}
