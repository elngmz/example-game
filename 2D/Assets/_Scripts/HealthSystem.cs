using System.Collections;
using UnityEngine;
using UnityEngine.UI; //gives access to UI backend
//using UnityStandardAssets._2D; //gives access to Standard 2D assets backend

public class HealthSystem : MonoBehaviour 

{

	//private Sprite [] HeartSprites; //an array/list of heart sprites - why? Cause we will be changing them according to the curHealth variable

	public Sprite m_img0;

	public Sprite m_img1;

	public Sprite m_img2;

	public Sprite m_img3;

	public Image m_HeartUI; // UI system = Canvas in Editor

	//private PlatformerCharacter2D player; //private access to the character controller script

	private CustomCharacterController player;


	void Start () 

	{

	
		player = GameObject.FindGameObjectWithTag ( "Player" ).GetComponent<CustomCharacterController> (); //find the character controller and get the PlatformerCharacter2D script

	}

	#region Health Change

	void Update () 

	{

		if ( player.m_curHealth == 3 )
		
		{

			m_HeartUI.sprite = m_img3;

		}

		if ( player.m_curHealth == 2 ) 
		
		{

			m_HeartUI.sprite = m_img2;

		}

		if ( player.m_curHealth == 1 ) 
		
		{

			m_HeartUI.sprite = m_img1;

		}

		if ( player.m_curHealth == 0 ) 
		
		{

			m_HeartUI.sprite = m_img0;

		}


		
	}

	#endregion


}
