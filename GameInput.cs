using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handle the input for the whole game
public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    //the event activated when a certain button is pressed
    public event EventHandler OnInteract; //f
    public event EventHandler OnInteractAlt;//e

    private InputActions inputActions;

    private void Awake()
    {
        inputActions = new InputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.InteractAlt.performed += InteractAlt_performed;

        Instance = this;
    }

    private void InteractAlt_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlt?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteract?.Invoke(this, EventArgs.Empty);
    }

    //returns a Vector 2 containing the direction the player moved in
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }

}
