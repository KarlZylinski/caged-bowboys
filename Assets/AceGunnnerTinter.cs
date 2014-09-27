using UnityEngine;
using System.Collections;

public class AceGunnnerTinter : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		var score_obj = GameObject.Find("Score");
		var winner = 0;

		if (score_obj != null)
		{
			var score = score_obj.GetComponent<ScoreKeeper>().Score;

			var top = 0;

			for(var i = 0; i < 4; ++i)
			{
				if (score[i] > top)
				{
					top = score[i];
					winner = i;
				}
			}
		}

		GetComponent<SpriteRenderer>().color = TintColor(winner);
	}


	private Color TintColor(int i)
	{
		if (i == 0)
			return Color.white;

		if (i == 1)
			return Color.green;

		if (i == 2)
			return Color.red;

		if (i == 3)
			return Color.yellow;

		return Color.white;
	}

	void Update()
	{
		if (Input.GetKeyDown("joystick button 0")
				|| Input.GetKeyDown("joystick button 1")
				|| Input.GetKeyDown("joystick button 2")
				|| Input.GetKeyDown("joystick button 3"))
		{
			Application.LoadLevel(0);
		}
	}
}
