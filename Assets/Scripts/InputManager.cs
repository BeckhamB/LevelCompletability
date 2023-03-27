using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private PlayerMovement playerMovement;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        playerMovement = new PlayerMovement();

    }
    private void OnEnable()
    {
        playerMovement.Enable();
    } 

    private void OnDisable()
    {
        playerMovement.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerMovement.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerMovement.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        return playerMovement.Player.Jump.triggered;
    }
    
    public bool PlayerPinged()
    {
        return playerMovement.Player.Ping.triggered;
    }
    
    public bool PlayerReturn()
    {
        return playerMovement.Player.Return.triggered;
    }

    public bool PlayerSwapCam()
    {
        return playerMovement.Player.SwapCamera.triggered;
    }

    public bool PingWheelPressed()
    {
        return playerMovement.Player.PingWheel.triggered;
    }

    public bool MouseClicked()
    {
        return playerMovement.Player.Click.triggered;
    }
    
    public bool EnterPressed()
    {
        return playerMovement.Player.Enter.triggered;
    }
}
