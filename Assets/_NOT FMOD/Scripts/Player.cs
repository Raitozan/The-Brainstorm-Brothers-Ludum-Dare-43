﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
	public float jumpHeight;
	public float timeToJumpApex;
	public float accelerationTimeAirborne;
	public float accelerationTimeGrounded;
	public float moveSpeed;
	public float crouchModificator;

	bool isCrouching;

	float gravity;
	float jumpVelocity;

	Vector3 velocity;
	float velocityXSmoothing;

	Controller2D controller;

	void Start ()
	{
		controller = GetComponent<Controller2D>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (controller.collisions.above || controller.collisions.below)
			velocity.y = 0;

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if(Input.GetButtonDown("Jump") && controller.collisions.below)
			velocity.y = jumpVelocity;

		if (Input.GetButtonDown("Crouch"))
		{
			isCrouching = true;
			controller.Crouch();
		}
		if (!Input.GetButton("Crouch") && isCrouching)
		{
			controller.Uncrouch(ref isCrouching);
		}

		float targetVelocityX = input.x * moveSpeed;
		if (isCrouching)
			targetVelocityX *= crouchModificator;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
