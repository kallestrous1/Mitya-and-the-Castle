using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    Animator ani;
    float holdDuration;
    bool resetWeapon;

    private bool attackPressed;
    private float UpOrDownTilt;
    private bool heavy;

    public PlayerWeapon playerWeapon;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    #region InputHandlers
    private void OnEnable()
    {
        InputManager.Instance.AttackPressed += OnAttackPressed;
        InputManager.Instance.AttackReleased += OnAttackReleased;
        InputManager.Instance.HeavyAttackPressed += setHeavyModifier;
        InputManager.Instance.HeavyAttackReleased += unsetHeavyModifier;
    }

    private void OnDisable()
    {
        InputManager.Instance.AttackPressed -= OnAttackPressed;
        InputManager.Instance.AttackReleased -= OnAttackReleased;
        InputManager.Instance.HeavyAttackPressed -= setHeavyModifier;
        InputManager.Instance.HeavyAttackReleased -= unsetHeavyModifier;
    }

    private void OnAttackPressed()
    {
        if(GameStateManager.instance.gameState != GameState.Play)
        {
            return;
        }
        attackPressed = true;
        HandleAttack();
    }

    private void OnAttackReleased()
    {
        attackPressed = false;
    }

    private void setHeavyModifier()
    {
        heavy = true;
    }

    private void unsetHeavyModifier()
    {
        heavy = false;
    }

    #endregion

    private void HandleAttack()
    {
        UpOrDownTilt = InputManager.Instance.UpOrDownTilt;
        playerWeapon = GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<PlayerWeapon>();

        if (heavy)
        {
            if (playerWeapon.activeWeapon)
            {
                /* if (playerWeapon.activeWeapon.heavyAttackSound)
                 {
                     AudioManager.Instance.Play(playerWeapon.activeWeapon.heavyAttackSound);
                 }*/
            }
            ani.SetTrigger("HeavyAttack");
            return;
        }
        if (playerWeapon.activeWeapon)
        {
            if (playerWeapon.activeWeapon.swingSound)
            {
                AudioManager.Instance.Play(playerWeapon.activeWeapon.swingSound);
            }
        }
        if (UpOrDownTilt == 1)
        {
            ani.SetTrigger("UpAttack");
        }
        else if (UpOrDownTilt == -1)
        {
            ani.SetTrigger("DownAttack");
        }
        else
        {
            ani.SetTrigger("Attack");
        }
    }
}
