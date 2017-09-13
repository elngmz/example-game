using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[ RequireComponent (typeof ( AudioSource) ) ]

public class ChangeScene : MonoBehaviour 

{

	public AudioClip uiSfx;

	AudioSource audio;

	void Start () {

		audio = GetComponent<AudioSource> ();

	}

	public void ChangeToScene ( int sceneToChangeTo ) {

		StartCoroutine ( "Delay" );
		SceneManager.LoadScene ( sceneToChangeTo );
		
	}

	IEnumerator Delay () {

		audio.PlayOneShot ( uiSfx, 1.0f );
		yield return new WaitForSeconds ( 1 );

	}
}
