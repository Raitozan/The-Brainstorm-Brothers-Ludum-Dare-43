﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityChooser : MonoBehaviour
{
	public Sprite dash, burn, climb, doubleJump;

	public Canvas canvas;
	public Image capacity1, capacity2;

	public Player player;

	public bool waitingForInput = false;

	public int abilityId;

	private void Update()
	{
		if (waitingForInput)
		{
			if (Input.GetButton("Ability1"))
			{
				player.ability1 = abilityId;
				Time.timeScale = 1;
				GameManager.instance.gamePaused = false;
				Destroy(gameObject);
			}
			if (Input.GetButton("Ability2"))
			{
				player.ability2 = abilityId;
				Time.timeScale = 1;
				GameManager.instance.gamePaused = false;
				Destroy(gameObject);
			}
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			player = collision.gameObject.GetComponent<Player>();

			switch (player.ability1)
			{
				case 0:
					capacity1.sprite = dash;
					break;
				case 1:
					capacity1.sprite = burn;
					break;
				case 2:
					capacity1.sprite = climb;
					break;
				case 3:
					capacity1.sprite = doubleJump;
					break;
			}
			switch (player.ability2)
			{
				case 0:
					capacity2.sprite = dash;
					break;
				case 1:
					capacity2.sprite = burn;
					break;
				case 2:
					capacity2.sprite = climb;
					break;
				case 3:
					capacity2.sprite = doubleJump;
					break;
			}

			canvas.gameObject.SetActive(true);

			Time.timeScale = 0;
			GameManager.instance.gamePaused = true;
			waitingForInput = true;
		}
	}
}