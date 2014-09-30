using System;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int PlayerNum = 0;
    private SpriteRenderer _renderer;
    private Transform _player;
    private PlayerControl _player_control;

    private Color TintColor()
    {
        if (PlayerNum == 0)
            return Color.white;

        if (PlayerNum == 1)
            return new Color(0.5f, 0.9f, 0.6f);

        if (PlayerNum == 2)
            return new Color(0.9f, 0.5f, 0.6f);

        if (PlayerNum == 3)
            return new Color(0.26f, 0.86f, 0.89f);

        throw new NotImplementedException();
    }

    private void Start()
    {
        var player = GameObject.Find("Player " + (PlayerNum + 1).ToString());

        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        _player = player.transform;
        _player_control = player.GetComponent<PlayerControl>();

        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = false;
    }

    public static Bounds OrthographicBounds(Camera camera)
    {
        var screen_aspect = (float)Screen.width / (float)Screen.height;
        var camera_height = camera.orthographicSize * 2;
        var bounds = new Bounds(
            camera.transform.position,
            new Vector3(camera_height * screen_aspect, camera_height, 20));
        return bounds;
    }

	void Update ()
	{
	    var camera_bounds = OrthographicBounds(Camera.main);
        _renderer.enabled = !camera_bounds.Contains(_player.position) && !_player_control.Dead;

	    var player_direction = (_player.position - transform.position).normalized;
	    transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(player_direction.y, player_direction.x) * Mathf.Rad2Deg);

	    var offset = 20.0f/100.0f;

        var x_pos = Mathf.Clamp(_player.position.x, camera_bounds.min.x + offset, camera_bounds.max.x - offset);
        var y_pos = Mathf.Clamp(_player.position.y, camera_bounds.min.y + offset, camera_bounds.max.y - offset);

	    transform.position = new Vector3(x_pos, y_pos, transform.position.z);
	    _renderer.color = TintColor();
	}
}
