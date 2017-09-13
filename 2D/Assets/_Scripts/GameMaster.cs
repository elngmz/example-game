using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {

	public int m_points;

	public Text m_pointsText;

	
	// Update is called once per frame
	void Update () {

		m_pointsText.text = ("Points: " + m_points);
		
	}
}
