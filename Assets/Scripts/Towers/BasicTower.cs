﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicTower : MonoBehaviour
{
	public bool enabled = false;
	public int fireRate;
	private int fireCooldown;
	public double fireRange;
	public AbstractBullet bulletType;
	public Field fieldPlacedOn;

	
	public virtual void FixedUpdate ()
	{
		if (enabled) {
			fireCooldown++;
			if (fireCooldown >= fireRate) {
				fire ();
			}
		}
	}
	
	public virtual void fire ()
	{
		GameObject[] creeps = GameObject.FindGameObjectsWithTag ("Creep");
		double minDist = fireRange;
		BasicCreep closest = null;
		foreach (GameObject gameObject in creeps) {
			BasicCreep creep = gameObject.GetComponent<BasicCreep> ();
			if (creep != null) {
				if (getDistance (creep) < minDist) {
					if (!bulletType.announcesDamage || !creep.dieing ()) {
						closest = creep;
						minDist = getDistance (creep);
					}
				} 
			}
		}
		
		if (closest != null) {
			shootAt (closest);
		}
	}
	
	public virtual void shootAt (BasicCreep creep)
	{
		AbstractBullet bullet = Instantiate (bulletType, this.transform.position, Quaternion.identity) as AbstractBullet;
		bullet.target (creep);
		bullet.transform.parent = this.transform;
		fireCooldown = 0;
	}
	
	public virtual double getDistance (BasicCreep creep)
	{
		return Vector3.Distance (this.transform.position, creep.transform.position);
	}

	public virtual int getCost ()
	{
		return 50;
	}
}
