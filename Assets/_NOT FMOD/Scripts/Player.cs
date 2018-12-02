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

    private bool isMoving = false;
    bool isCrouching;

  


	float gravity;
	float jumpVelocity;

	Vector3 velocity;
	float velocityXSmoothing;
    float sfxVelocity = 0f;

    Controller2D controller;

    private FMOD.Studio.EventInstance robotMovementSFX;
    private FMOD.Studio.EventInstance crouchingSFX;
	delegate void Ability();
	List<Ability> abilities = new List<Ability>();
	public int ability1;
	public int ability2;

	void Start ()
	{
		controller = GetComponent<Controller2D>();

		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		abilities.Add(Dash);
		abilities.Add(Burn);
		abilities.Add(Climb);
		abilities.Add(DoubleJump);
		ability1 = -1;
		ability2 = -1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!GameManager.instance.gamePaused)
		{
			if (controller.collisions.above || controller.collisions.below)
				velocity.y = 0;

			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump") && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
            FMODUnity.RuntimeManager.PlayOneShot(FMODPaths.ROBOT_JUMP, GetComponent<Transform>().position);  // jump sound      
        }

        sfxVelocity = velocity.y;

			if (Input.GetButtonDown("Crouch"))
			{
				isCrouching = true;
				controller.Crouch();
            StartCrouchingSFX();
			}
			if (!Input.GetButton("Crouch") && isCrouching)
			{
				controller.Uncrouch(ref isCrouching);
            StopCrouchingSFX();
        }

			float targetVelocityX = input.x * moveSpeed;
			if (isCrouching)
				targetVelocityX *= crouchModificator;
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
			controller.Move(velocity * Time.deltaTime);

			if (Input.GetButtonDown("Ability1") && ability1 != -1)
				abilities[ability1]();
			if (Input.GetButtonDown("Ability2") && ability2 != -1)
				abilities[ability2]();
		}
	}


        if (targetVelocityX != 0 && (!Input.GetButtonDown("Jump"))) // checking if player moving, then executing MovementSFX
        {
            if (!isMoving)
            {
                MovementSFX();
                isMoving = true;
            }
        }

        if (targetVelocityX == 0 || Input.GetButtonDown("Jump") || sfxVelocity != 0)
        {
            MovementSFXStop();
            isMoving = false;
        }

    }

	public void Dash()
	{
		Debug.Log("Dash");
	}

	public void Burn()
	{
		Debug.Log("Burn");
	}

	public void Climb()
	{
		Debug.Log("Climb");
	}

	public void DoubleJump()
	{
		Debug.Log("DoubleJump");
	}

	public void changeAbility(int ind)
	{
		ability1 = ind;
	}

    #region SFX methods

    void MovementSFX() // starts movement Loop
    {
        robotMovementSFX = FMODUnity.RuntimeManager.CreateInstance(FMODPaths.ROBOT_MOVE);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(robotMovementSFX, this.transform, GetComponent<Rigidbody>());
        robotMovementSFX.start();
    }

    void MovementSFXStop()
    {
        robotMovementSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT); //stopping movement sound when jumping
        robotMovementSFX.release();
    }

    void StartCrouchingSFX()
    {
        crouchingSFX = FMODUnity.RuntimeManager.CreateInstance(FMODPaths.CROUCH);
        crouchingSFX.start();
    }

    void StopCrouchingSFX()
    {
        crouchingSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        crouchingSFX.release();
    }

    #endregion



}
