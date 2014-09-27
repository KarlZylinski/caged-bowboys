using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
	public int[] Score = { 0, 0, 0, 0 };

	public void Start () {
		var scores = GameObject.FindGameObjectsWithTag("Score");

		foreach (var s in scores)
		{
			if (s != gameObject)
				Destroy(s);
		}

		DontDestroyOnLoad(gameObject);
	}
}
