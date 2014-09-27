using System.Linq;
using UnityEngine;

public class FollowPlayers : MonoBehaviour
{
	public float CameraFollowSpeed = 100.0f;
	private Vector2 _wanted_pos;

	private Vector2 CalculateWantedPos()
	{
		var tracked = FindObjectsOfType<TrackedByCamera>();
		Vector3 first_pos = tracked[0].transform.position;
		var num_tracked = 1;
		var tracked_added = new Vector2(0, 0);

		for (var i = 1; i < tracked.Count(); ++i)
		{
			var t = tracked[i];

			var controller = t.GetComponent<PlayerControl>();

			if (controller == null || !controller.Dead || (controller.Dead && Time.time + 2.0 > controller.TimeOfDeath))
			{
				Vector2 diff = t.transform.position - first_pos;
				tracked_added += diff;
				++num_tracked;
			}
		}

		const float cameraHalf = 1.5f;
		Vector2 first_pos_v2 = first_pos;
		var wanted_pos = first_pos_v2 + (num_tracked == 0 ? new Vector2(0, 0) : (tracked_added * (1.0f/num_tracked)));

		const float halfBgWidth = 455.0f/100.0f;

		if (wanted_pos.x - cameraHalf <= -halfBgWidth)
			wanted_pos = new Vector2(-halfBgWidth + cameraHalf, wanted_pos.y);
		else if (wanted_pos.x + cameraHalf >= halfBgWidth)
			wanted_pos = new Vector2(halfBgWidth - cameraHalf, wanted_pos.y);

		return wanted_pos;
	}

	public void Update ()
	{
		_wanted_pos = CalculateWantedPos();

		Vector2 current_pos = camera.transform.position;

		var distance_to_wanted = _wanted_pos - current_pos;
		var distance_to_move = distance_to_wanted*Time.deltaTime;

		camera.transform.position += new Vector3(distance_to_move.x, distance_to_move.y, 0);
	}
}
