using System.Collections;//add to use IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement; // add to load scenes

[ RequireComponent ( typeof ( AudioSource ) ) ]
public class CustomCharacterController : MonoBehaviour 

{

	public float m_maxSpeed;
	public float m_speed; //how fast character walks
	public float m_jumpForce; //how much force we add when character jumps

	public bool m_grounded; //checks if the player is on a platform
	public bool m_deathCheck; //checks if player is dead
	public bool m_hurt; //checks if player is damaged

	private Rigidbody2D m_rigiBody; //gives access to the RigidBOdy2D of the controller gameobject

	private Animator m_anim; //gives access to the Animator of the controller gameobject

	public AudioClip m_jumping; // jumping audio file

	AudioSource audio; //audio source setup

	private GameMaster m_gm; //GameMaster script
	public AudioClip m_coinCollect; // coin collect audio file

	//Stats
	public int m_curHealth; //current health integer
	public int m_maxhealth = 3; // max health integer = full health

	public AudioClip m_damageSfx;

	private Enemy m_enemy;

	public float m_duration;

	public GameObject m_respawnPoint;

	public GameObject m_goOverlay;

	#region Initialiation

	void Start () 

	{

		m_rigiBody = gameObject.GetComponent<Rigidbody2D> (); //getting RigidBOdy2D info from gameobject

		m_anim = gameObject.GetComponent<Animator>(); //getting Animator info from gameobject

		audio = GetComponent<AudioSource> (); // access audio source component

		m_gm = GameObject.FindGameObjectWithTag ( "GameMaster" ).GetComponent<GameMaster> (); // access the GameMaster script from the GameMaster gameobject

		m_curHealth = m_maxhealth; // at start of game, health is always full

		m_goOverlay.SetActive ( false );

		m_enemy = GameObject.FindGameObjectWithTag ( "Enemy" ).GetComponent<Enemy> (); //access Enemy script on enemy gameobject

	}
		
	#endregion

	#region Movement and Health

	void Update () 

	{

		Debug.Log ("max speed " + m_maxSpeed + ", speed:" + m_speed);
		m_anim.SetBool ( "IsGrounded", m_grounded ); //setting our IsGrounded animator parameter
		m_anim.SetFloat ( "Speed", Mathf.Abs (m_rigiBody.velocity.x)); //setting our Speed animator parameter || Mathf.Abs make it so that when we move left in negative, it's always a positive number
		//anim.SetBool ("IsAlive", deathCheck); // setting IsAlive parameter
		m_anim.SetBool ( "IsDamaged", m_hurt ); //setting IsDamaged parameter


		//adds integrated input to move right and left arrow keys
		float h = Input.GetAxis( "Horizontal" ); 

		//flipping character's sprite to face correct direction on X axis
		if (Input.GetAxis ( "Horizontal" ) < -.001f) 
		
		{

			transform.localScale = new Vector3 ( -2.2f, 2.2f, 2.2f );

		}

		//Moving the character
		if (m_grounded) 
		
		{

			m_rigiBody.AddForce (( Vector2.right * m_speed ) * h); //moves the player when you move left and right

		}

		if (Input.GetAxis ( "Horizontal" ) > .001f) 
		
		{

			transform.localScale = new Vector3 ( 2.2f, 2.2f, 2.2f );



		}

		if ( Input.GetKeyDown ( KeyCode.Space ) && m_grounded ) 
		
		{

			m_rigiBody.AddForce ( Vector2.up * m_jumpForce );

			audio.PlayOneShot ( m_jumping, 1.0f ); //play sfx

		}

		if ( !m_grounded ) 
		
		{

			m_speed = 300f;

		} else 
		
		{

			m_speed = 400f;
		}


		if ( m_curHealth > m_maxhealth ) 
		
		{

			m_curHealth = m_maxhealth; //always sets health to maximum at start of game

		}

		if ( m_curHealth <= 0 ) 
		
		{

			SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );

			StartCoroutine ( "EnumDelayedRestart" ); //if health is less than or equal to 0, restart the scene with a delay (IEnumerator)

		}

		if ( Input.GetKeyDown( KeyCode.R ) && m_deathCheck ) 
		
		{

			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		}

		if ( !m_deathCheck ) 
		
		{

			Time.timeScale = 1;

		}


	}

	#endregion

	#region Lock Movement

	void FixedUpdate () 

	{

		Vector3 easeVelocity = m_rigiBody.velocity;
		easeVelocity.y = m_rigiBody.velocity.y;
		easeVelocity.z = 0.0f;
		easeVelocity.x *= 0.75f;

		if (m_grounded) 
		
		{

			m_rigiBody.velocity = easeVelocity;

		}

		//Limiting the speed of the character*
		if ( m_rigiBody.velocity.x > m_maxSpeed ) 
		
		{

			m_rigiBody.velocity = new Vector2 ( m_maxSpeed, 0f );

		}

		if ( m_rigiBody.velocity.x < -m_maxSpeed ) 
		
		{

			m_rigiBody.velocity = new Vector2 ( -m_maxSpeed, 0f );

		}
	}

	#endregion

	#region Interactions

	void OnTriggerEnter2D(Collider2D col) 

	{

		if ( col.CompareTag ( "Coin" )) 
		
		{ // if player collides with a gameobject of tag "coin" then...

			Destroy (col.gameObject); //delete the gameobject

			audio.PlayOneShot (m_coinCollect, .45f); // play coin collect sfx

			m_gm.m_points += 1; //add a point to coin collection

		}

		//respawning character
		if ( col.CompareTag ("Fall Detector" )) 
		
		{

			transform.position = m_respawnPoint.transform.position; // make character's position the same as respawn point


		}
	}

	#endregion

	#region Death and Restart
	 
	void Death () 

	{

		m_deathCheck = true;

		m_goOverlay.SetActive (true);

			
		if ( m_deathCheck ) 
		
		{

			Time.timeScale = 0;
		}
			
			//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


		}

		IEnumerator EnumDelayedRestart () 

		{
	
		yield return new WaitForSeconds ( m_duration ); //delay by float duration

			Death (); //execute Die method

		}
		

		public void Damage ( int dmg ) 

		{

			
			audio.PlayOneShot( m_damageSfx, 1.0f ); //play damage audio file

			m_curHealth -= dmg; // take -1 damage


		}

	#endregion
		
}