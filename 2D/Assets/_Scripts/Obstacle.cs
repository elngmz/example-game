using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Obstacle : MonoBehaviour 

{

	private CustomCharacterController m_player; //calling character controller script

	#region Inizialization

	void Start () 

	{

		m_player = GameObject.FindGameObjectWithTag ( "Player" ).GetComponent<CustomCharacterController> (); //accessing PlatformerCharacter2D script from tag "Player"

	}

	#endregion

	#region Damage

	void OnCollisionEnter2D ( Collision2D coll ) 

	{

		if ( coll.gameObject.tag == "Player" ) 
		
		{ //if there is a collision between the obstacle's trigger area and the player, then...

			m_player.m_hurt = true;

			m_player.Damage ( 1 ); //take one heart from character's health

			//StartCoroutine (player.Knockback (0.02f, 350, player.transform.position)); //start the IEnumerator Knockback, with specified values for each variable

		} else 
		
		{

			m_player.m_hurt = false;

		}

	}

	#endregion

	#region Damage Check

	void OnCollisionExit2D ( Collision2D coll ) 

	{

		if ( coll.gameObject.tag == "Player" ) { //if there is a collision between the obstacle's trigger area and the player, then...

			m_player.m_hurt = false;


		}

	}

	#endregion
		
}
