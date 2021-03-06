﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

	public Vector3 movement;
	public float speed;
	public int damage;
	public float lifeTime;
	public bool moving;
	public float distanceToMove;
	public Transform player;
	public bool destroyable;
	
	// Update is called once per frame
	void Update () {
		if (!moving)
		{
			if ((player.position - transform.position).magnitude <= distanceToMove)
				moving = true;
		}
		else
		{
			if (lifeTime != -1 && destroyable)
			{
				lifeTime -= Time.deltaTime;
				if (lifeTime <= 0)
					Destroy(gameObject);
			}
			Vector3 velocity = movement * speed * Time.deltaTime;

			transform.Translate(velocity);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Player p = collision.GetComponent<Player>();
			if (!p.isDashing)
			{
				GameManager.instance.playerEnergy -= damage;
				if(destroyable)
					Destroy(gameObject);
			}
		}
	}
}
