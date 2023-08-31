using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private CharacterController controller;
    [SerializeField] Transform cam;

    float rotationSpeed = 10f;
    float turnSmoothVelocity;
    private void Update()
    {
        BrackeysMove();
    }

    private void BrackeysMove()
    {
        //stransform.forward = Camera.main.transform.forward;
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = movementSpeed * Time.deltaTime;

        transform.position += moveDir * moveDistance;
        if (moveDir.magnitude >= .1f)
        {
            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 direction = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(direction.normalized * movementSpeed * Time.deltaTime);
        }
    }

    private void Move()
    {
        //stransform.forward = Camera.main.transform.forward;
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = movementSpeed * Time.deltaTime;

        transform.position += moveDir * moveDistance;
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            //rotation
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }
    }
    
    

}
