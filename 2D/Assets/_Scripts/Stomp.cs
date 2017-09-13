using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ RequireComponent ( typeof ( AudioSource ) ) ]
public class Stomp : MonoBehaviour 

{

	public AudioClip m_enemyDeath;
	public float m_delay;

	AudioSource audio;

	private CustomCharacterController m_player;

	void Start () 

	{

		audio = GetComponent<AudioSource> ();

		m_player = m_player = GameObject.FindGameObjectWithTag ( "Player" ).GetComponent<CustomCharacterController> (); 

	}

	#region Functionality

	void OnTriggerEnter2D(Collider2D other) 

	{

			if ( other.gameObject.tag == "Player" )
		
		{
				GetComponentInParent<BoxCollider2D> ().enabled = false;

				audio.PlayOneShot ( m_enemyDeath, 1.0f );

				Destroy ( transform.parent.gameObject ); //delete the parent of gameobject - the enemy

				m_player.m_hurt = false;

				StartCoroutine ( "EnumDeathDelay" );

		}

	}

	#endregion

	IEnumerator EnumDeathDelay () 

	{

		audio.PlayOneShot ( m_enemyDeath, 1.0f );

		yield return new WaitForSeconds ( m_delay );

		Destroy ( transform.parent.gameObject ); //delete the parent of gameobject - the enemy 
	
	}

}
