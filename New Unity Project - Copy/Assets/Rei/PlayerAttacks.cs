using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    Animator ani;
    float holdDuration;
    bool resetWeapon;

    public PlayerWeapon playerWeapon;

    void Start()
    {
        StartCoroutine(ChargeWeapon());
        ani = GetComponent<Animator>();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            playerWeapon = GameObject.FindGameObjectWithTag("PlayerWeapon").GetComponent<PlayerWeapon>();
            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                if(playerWeapon.activeWeapon)
                {
                   /* if(playerWeapon.activeWeapon.heavyAttackSound)
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
            if (Input.GetKey(KeyCode.UpArrow))
            {
                ani.SetTrigger("UpAttack");
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                ani.SetTrigger("DownAttack");
            }
            else
            {
                ani.SetTrigger("Attack");
            }
        }

        /*if (Input.GetKey(KeyCode.V))
        {
            holdDuration += Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.V))
        {
            if (holdDuration < 0.3f)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    ani.SetTrigger("UpAttack");
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    ani.SetTrigger("DownAttack");
                }
                else
                {
                    ani.SetTrigger("Attack");
                }
            }
            else
            {
                ani.SetFloat("ChargeWeapon", 2);
                resetWeapon = true;
                StartCoroutine(ResetWeapon());
            }
            holdDuration = 0;
        }*/
    }

    IEnumerator ChargeWeapon()
    {
        if (holdDuration > 0.3)
        {
            AddChargeWeapon();
        }
        yield return new WaitForSeconds(.02f);
        StartCoroutine(ChargeWeapon());
    }

    IEnumerator ResetWeapon()
    {
        if (ani.GetFloat("ChargeWeapon") > 0f && resetWeapon)
        {
            ani.SetFloat("ChargeWeapon", ani.GetFloat("ChargeWeapon") - 0.03f);
            yield return new WaitForSeconds(.01f);
            StartCoroutine(ResetWeapon());
        }
        else
        {
            resetWeapon = false;
        }
    }

    void AddChargeWeapon()
    {
        if (ani.GetFloat("ChargeWeapon") < 1.0f)
        ani.SetFloat("ChargeWeapon", ani.GetFloat("ChargeWeapon")+0.01f);       
    }

    public void setSwordColliderState(float state)
    {
       
        playerWeapon.SetPlayerWeaponHitboxState(state);
        
    }

    public void CastWeaponSpell()
    {
        playerWeapon.castBaseActiveSpell();
    }

}
