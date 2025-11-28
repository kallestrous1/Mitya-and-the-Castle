using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Animations;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private GameplayControls controls;

    // Movement
    public float Move { get; private set; }
    public float UpOrDownTilt { get; private set; }

    // Events for actions
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;

    public event Action OnDashPressed;

    public event Action AttackPressed;
    public event Action AttackReleased;

    public event Action HeavyAttackPressed;
    public event Action HeavyAttackReleased;

    public event Action SpellPressed;
    public event Action SpellReleased;

    public event Action InventoryPressed;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        controls = new GameplayControls();

        // Movement
        controls.Player.XMovement.performed += ctx => Move = ctx.ReadValue<float>();
        controls.Player.XMovement.canceled += ctx => Move = 0f;

        // Jump
        controls.Player.Jump.started += _ => OnJumpPressed?.Invoke();
        controls.Player.Jump.canceled += _ => OnJumpReleased?.Invoke();

        // Dash
        controls.Player.Dash.started += _ => OnDashPressed?.Invoke();

        // Attack
        controls.Player.BaseAttack.started += _ => AttackPressed?.Invoke();
        controls.Player.BaseAttack.canceled += _ => AttackReleased?.Invoke();

        controls.Player.HeavyAttackModifier.started += _ => HeavyAttackPressed?.Invoke();
        controls.Player.HeavyAttackModifier.canceled += _ => HeavyAttackReleased?.Invoke();

        //spells
        controls.Player.Spell.started += _ => SpellPressed?.Invoke();
        controls.Player.Spell.canceled += _ => SpellReleased?.Invoke();

        //tilt 
        controls.Player.UpOrDownTilt.performed += ctx => UpOrDownTilt = ctx.ReadValue<float>();
        controls.Player.UpOrDownTilt.canceled += ctx => UpOrDownTilt = 0f;

        //inventory
        controls.Player.Inventory.performed += _ => InventoryPressed.Invoke();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();
}
