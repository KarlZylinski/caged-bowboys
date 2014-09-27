using System.Linq;
using UnityEngine;

public class FollowPlayers : MonoBehaviour {
	public void Update ()
	{
		var tracked = FindObjectsOfType<TrackedByCamera>();
		Vector3 first_pos = tracked[0].transform.position;
		var num_tracked = 1;
		var tracked_added = new Vector2(0, 0);

		for (var i = 1; i < tracked.Count(); ++i)
		{
			Vector2 diff = tracked[i].transform.position - first_pos;
			tracked_added += diff;
			++num_tracked;
		}

		const float cameraHalf = 1.5f;
		Vector2 first_pos_v2 = first_pos;
		var wanted_pos = first_pos_v2 + (num_tracked == 0 ? new Vector2(0, 0) : (tracked_added * (1.0f/num_tracked)));

		const float halfBgWidth = 455.0f/100.0f;

		if (wanted_pos.x - cameraHalf <= -halfBgWidth)
			wanted_pos = new Vector2(-halfBgWidth + cameraHalf, wanted_pos.y);
		else if (wanted_pos.x + cameraHalf >= halfBgWidth)
			wanted_pos = new Vector2(halfBgWidth - cameraHalf, wanted_pos.y);

		camera.transform.position = new Vector3(wanted_pos.x, wanted_pos.y, -10.0f);
	}
}
