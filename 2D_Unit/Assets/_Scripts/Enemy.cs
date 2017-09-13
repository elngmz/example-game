using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 

{

	public LayerMask m_enemyMask;
	public float m_speed = 1f;
	Rigidbody2D m_myBody;
	Transform m_myPosition;
	float m_myWidth;
	float m_myHeight;



	#region  Initialization

	void Start () 

	{

		SpriteRenderer mySprite = this.GetComponent<SpriteRenderer> ();

		m_myPosition = this.transform;
		m_myBody = this.GetComponent<Rigidbody2D> ();
		m_myWidth = mySprite.bounds.extents.x; //gets the width of the sprite of the enemy
		m_myHeight = mySprite.bounds.extents.y; //gets the height of the sprite of the enemy

		
	}
	
	#endregion

	#region Raycast

	void FixedUpdate () 

	{

		//checking the ground in front of enemy
		Vector2 lineCastPos = m_myPosition.position.toVector2() - m_myPosition.right.toVector2() * m_myWidth + Vector2.up * m_myHeight;

		Debug.DrawLine ( lineCastPos, lineCastPos + Vector2.down );

		bool isGrounded = Physics2D.Linecast ( lineCastPos, lineCastPos + Vector2.down, m_enemyMask );

		Debug.DrawLine ( lineCastPos, lineCastPos - m_myPosition.right.toVector2() * .05f);

		bool isBlocked = Physics2D.Linecast ( lineCastPos, lineCastPos - m_myPosition.right.toVector2 () * .05f, m_enemyMask );



		// if there is no ground or if enemy is blocked, turn around

		//always move forward
		Vector2 myVelocity = m_myBody.velocity;
		myVelocity.x = m_myPosition.right.x * -m_speed;
		m_myBody.velocity = myVelocity;
	

		if ( !isGrounded || isBlocked ) 
		
		{

			Vector3 currRot = m_myPosition.eulerAngles;
			currRot.y += 180;
			m_myPosition.eulerAngles = currRot;


		}


	}

	#endregion

}
