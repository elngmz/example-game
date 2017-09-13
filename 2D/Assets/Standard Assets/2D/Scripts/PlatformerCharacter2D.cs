using System;
using UnityEngine;
using UnityEngine.SceneManagement; // add to load scenes
using System.Collections; //add to use IEnumerator


/*namespace UnityStandardAssets._2D

{
	[RequireComponent(typeof(AudioSource))]
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

		public AudioClip jumping; // jumping audio file
		AudioSource audio; //audio source setup

		private GameMaster gm; //GameMaster script
		public AudioClip coinCollect; // coin collect audio file

		//Stats
		public int curHealth; //current health integer
		public int maxhealth = 3; // max health integer = full health

		public AudioClip damageSfx;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

        }

		void Start() {

			audio = GetComponent<AudioSource> (); // access audio source component

			gm = GameObject.FindGameObjectWithTag ("GameMaster").GetComponent<GameMaster> (); // access the GameMaster script from the GameMaster gameobject

			curHealth = maxhealth; // at start of game, health is always full

		}

		void Update () {

			if (curHealth > maxhealth) {

				curHealth = maxhealth; //always sets health to maximum at start of game

			}

			if (curHealth <= 0) {

				StartCoroutine ("DelayedRestart"); //if health is less than or equal to 0, restart the scene with a delay (IEnumerator)

			}


		}


        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
				audio.PlayOneShot (jumping, 1.0f); //play sfx
            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }


		void OnTriggerEnter2D(Collider2D col) {

			if (col.CompareTag ("Coin")) { // if player collides with a gameobject of tag "coin" then...

				Destroy (col.gameObject); //delete the gameobject
				audio.PlayOneShot (coinCollect, 1.0f); // play coin collect sfx
				gm.points += 1; //add a point to coin collection

			}


		}

		void Death () {

			SceneManager.LoadScene ("test1"); //load scene 1


		}

		IEnumerator DelayedRestart () {

			yield return new WaitForSeconds (1); //delay by 1 second
			Death (); //execute Die method

		}

		public void Damage (int dmg) {

			audio.PlayOneShot(damageSfx, 1.0f); //play damage audio file
			GetComponent<Animation>().Play("alien_damage"); //play damage animation
			curHealth -= dmg; // take -1 damage

		}

		public IEnumerator Knockback(float knockDur, float knockbackPwr, Vector3 knockbackDir) { //knockDur is how long we will add force for knockback, knockbackPwr is the amount of force, knockbackDir is the direction we are sending the player

			float timer = 0; //counting the time elapsed of the method

			while (knockDur > timer) { //while the length of the added force is greater than 0...

				timer += Time.deltaTime; //add time to the timer, counts in seconds

				m_Rigidbody2D.AddForce (new Vector3 (knockbackDir.x * -100, knockbackDir.y * knockbackPwr, transform.position.z)); //add force to move character's position to the left on the X axis and slightly on Y, no change on Z
			}

			yield return 0; //when while condition is not met, the coroutine stops


			}


    }

}
*/


