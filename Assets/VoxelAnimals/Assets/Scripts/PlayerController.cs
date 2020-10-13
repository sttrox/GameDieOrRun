﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3;
    public float jumpForce = 300;
    public float timeBeforeNextJump = 1.2f;
    private float canJump = 0f;
    Animator anim;
    CharacterController characterController;
    private float _gravityForce = -9.8f;

    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    //private  Queue<GameObject>
    void Update()
    {
        ControllPlayer();
        GameGravity();
    }

    private void GameGravity()
    {
        if (characterController.isGrounded)
        {
            //characterController.detectCollisions;
        }
    }
    IEnumerator ProcessingWalking()
    {

        yield return null;
        for (float ft = 1f; ft >= 0; ft -= 0.1f) 
        {
           // Color c = renderer.material.color;
           // c.a = ft;
           // renderer.material.color = c;
           // yield return new WaitForSeconds(.1f);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _gravityForce = !characterController.isGrounded ? -9.8f * Time.deltaTime : -9.8f;
        if (characterController.isGrounded)
        {
            ManagerCell.instance.StepOn(hit);
            //hit.gameObject.BroadcastMessage("StepOn", SendMessageOptions.RequireReceiver);
        }
        else
        {
        }
    }

    void ControllPlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");


        Vector3 movement =
            new Vector3(moveHorizontal * movementSpeed * 0.01f, 0f, moveVertical * movementSpeed * 0.01f);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            anim.SetInteger("Walk", 1);
        }
        else
        {
            anim.SetInteger("Walk", 0);
        }

        movement.y  = _gravityForce;
        characterController.Move(movement);


        if (Input.GetButtonDown("Jump") && Time.time > canJump)
        {
            //characterController.AddForce(0, jumpForce, 0);
            canJump = Time.time + timeBeforeNextJump;
            anim.SetTrigger("jump");
        }
    }
}