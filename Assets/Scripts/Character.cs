using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float movementSpeed = 3;

    private float canJump = 0f;
    public float timeBeforeNextJump = 1.2f;

    Animator anim;

    CharacterController characterController;

    private float _gravityForce = -3.25f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlCharacter();
        GameGravity();
    }

    private void GameGravity()
    {
        if (characterController.isGrounded)
        {
            //characterController.detectCollisions;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (characterController.isGrounded)
        {
            ManagerLifeCells.instance.StepOn(hit);
            //hit.gameObject.BroadcastMessage("StepOn", SendMessageOptions.RequireReceiver);
        }
        else
        {
        }
    }

    protected abstract InputDataDto GetInputParameters();

    private void ControlCharacter()
    {
        var inputData = GetInputParameters();


        Vector3 movement =
            new Vector3(inputData.HorizontalInput * movementSpeed * 0.01f, 0f,
                inputData.VerticalInput * movementSpeed * 0.01f);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            anim.SetInteger("Walk", 1);
        }
        else
        {
            anim.SetInteger("Walk", 0);
        }

        movement.y = _gravityForce * Time.deltaTime;
        characterController.Move(movement);


        if (inputData.JumpInput && Time.time > canJump)
        {
            //characterController.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("jump");
        }
    }

    protected readonly struct InputDataDto
    {
        public InputDataDto(float horizontalInput, float verticalInput, bool jumpInput)
        {
            HorizontalInput = horizontalInput;
            VerticalInput = verticalInput;
            JumpInput = jumpInput;
        }

        public float HorizontalInput { get; }
        public float VerticalInput { get; }
        public bool JumpInput { get; }
    }
}