using System.Linq;
using UnityEngine;

public class ArmControl : MonoBehaviour
{
	public GameObject BulletPrototype;
	public float ShootSpeed = 200.0f;
	private PlayerControl _player_control;
	private Transform _bullet_spawn_point;
	public float ShootCooldown = 0.5f;
	private float _can_shoot_at;
	private bool[] _loaded;
	private int _current_barrallel;
	private bool _fire_held_last_frame;
	private bool _reloading;

	public void Start()
	{
		_reloading = false;
		_fire_held_last_frame = false;
		_current_barrallel = 0;
		_loaded = new bool[4];
		
		for (var i = 0; i < 4; ++i)
			_loaded[i] = true;

		_player_control = transform.parent.GetComponent<PlayerControl>();
		_bullet_spawn_point = transform.FindChild("BulletSpawnPoint");
		_can_shoot_at = 0;
	}

	private bool ClipEmpty()
	{
		return _loaded.All(x => !x);
	}

	private void Fire()
	{
		if (!_reloading && _loaded[_current_barrallel])
		{
			var bullet = (GameObject)Instantiate(BulletPrototype);
			bullet.transform.position = _bullet_spawn_point.transform.position;
			bullet.layer = 13 + int.Parse(_player_control.PlayerNum) - 1;
			var direction = (new Vector2(_bullet_spawn_point.transform.position.x, _bullet_spawn_point.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
			bullet.rigidbody2D.AddForce(direction * ShootSpeed);
			_can_shoot_at = Time.time + ShootCooldown;
			_loaded[_current_barrallel] = false;
		}

		if (ClipEmpty())
			_reloading = true;

		_current_barrallel = (_current_barrallel + 1)%4;
	}

	public void ReloadBarrallel(int index)
	{
		if (_loaded[index])
			return;

		_loaded[index] = true;
		// PLAY SOUND
	}

	public void Update()
	{
		var look = new Vector2(Input.GetAxis(_player_control.GetInputName("LookX")), Input.GetAxis(_player_control.GetInputName("LookY")));

		if (look.magnitude > 0.4f)
			transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(look.y, look.x * transform.parent.localScale.x) * Mathf.Rad2Deg);

		if (_reloading)
		{
			if (Input.GetButtonDown(_player_control.GetInputName("Reload1")))
				ReloadBarrallel(0);
			else if (Input.GetButtonDown(_player_control.GetInputName("Reload2")))
				ReloadBarrallel(1);
			else if (Input.GetButtonDown(_player_control.GetInputName("Reload3")))
				ReloadBarrallel(2);
			else if (Input.GetButtonDown(_player_control.GetInputName("Reload4")))
				ReloadBarrallel(3);
		}
			
		var shoot_pressed = Input.GetAxis(_player_control.GetInputName("Shoot")) > 0.5f;
		
		if (shoot_pressed && !_fire_held_last_frame && Time.time > _can_shoot_at)
		{
			if (_reloading && !ClipEmpty())
			{
				_current_barrallel = Random.Range(0, 3);
				_reloading = false;
			}

			Fire();
		}

		_fire_held_last_frame = shoot_pressed;
	}
}
