using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
	public void Start()
	{
		Screen.showCursor = false;
	}

	void Update()
	{
		if (Input.GetKeyDown("joystick button 0")
			|| Input.GetKeyDown("joystick button 1")
			|| Input.GetKeyDown("joystick button 2")
			|| Input.GetKeyDown("joystick button 3"))
		{
			Application.LoadLevel(1);
		}

		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
