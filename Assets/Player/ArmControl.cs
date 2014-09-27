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
	public bool DeathHandled;
	private PlayerGUIScript _gui;

	public void Start()
	{
		_player_control = transform.parent.GetComponent<PlayerControl>();
		_gui = GameObject.Find(_player_control.PlayerName() + "GUI").GetComponent<PlayerGUIScript>();
		_reloading = false;
		_fire_held_last_frame = false;
		_current_barrallel = 0;
		_loaded = new bool[4];
		DeathHandled = false;
		
		for (var i = 0; i < 4; ++i)
			_loaded[i] = true;

		_bullet_spawn_point = transform.FindChild("BulletSpawnPoint");
		_can_shoot_at = 0;
	}

	private bool ClipEmpty()
	{
		return _loaded.All(x => !x);
	}

	public bool Reloading()
	{
		return _reloading;
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
		{
			_reloading = true;
			_gui.Show();
		}

		_current_barrallel = (_current_barrallel + 1)%4;
	}

	public void ReloadBarrallel(int index)
	{
		if (_loaded[index])
			return;

		_gui.LoadBullet(index + 1);

		_loaded[index] = true;
		// PLAY SOUND
	}

	public void Update()
	{
		if (_player_control.Dead && !DeathHandled)
		{
			transform.parent = null;
			gameObject.AddComponent<Rigidbody2D>();
			gameObject.AddComponent<BoxCollider2D>();
			DeathHandled = true;
			rigidbody2D.AddForce(new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f))* 200.0f);
			transform.Rotate(new Vector3(0, 0, Random.Range(-25f, 25f)));
			Destroy(this);
		}

		if (_player_control.Dead)
			return;

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
				_gui.Hide();
			}

			Fire();
		}

		_fire_held_last_frame = shoot_pressed;
	}
}
