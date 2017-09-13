using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour 

{

	private CustomCharacterController m_player; //gives access to the script

	// Use this for initialization
	void Start () 

	{

		m_player = gameObject.GetComponentInParent<CustomCharacterController> (); //characontroller is parent... get component from it

	}

	void OnTriggerEnter2D ( Collider2D col ) 

	{
		
		m_player.m_grounded = true;

	}

	void OnTriggerStay2D( Collider2D col ) 

	{
		m_player.m_grounded = true;

	}

	void OnTriggerExit2D ( Collider2D col ) 

	{
		m_player.m_grounded = false;

	}
}
