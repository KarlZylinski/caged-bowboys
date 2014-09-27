using UnityEngine;
using System.Collections;

public class ControlScreen : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown("joystick button 0")
			|| Input.GetKeyDown("joystick button 1")
			|| Input.GetKeyDown("joystick button 2")
			|| Input.GetKeyDown("joystick button 3"))
		{
			Application.LoadLevel(2);
		}
	}
}
