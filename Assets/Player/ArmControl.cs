using System.Linq;
using System.Text;
using UnityEngine;

public class ArmControl : MonoBehaviour
{
	public GameObject BulletPrototype;
	public GameObject ArmPrototype;
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
	private bool _set_shoot_button_state;
	private AudioSource _audio_source;
	public AudioClip[] FireSounds;
	public AudioClip EmptyClipSound;
	public AudioClip ReloadSound;

	public void Start()
	{
		_player_control = transform.parent.GetComponent<PlayerControl>();
		_gui = GameObject.Find(_player_control.PlayerName() + "GUI").GetComponent<PlayerGUIScript>();
		_reloading = false;
		_fire_held_last_frame = _set_shoot_button_state;
		_current_barrallel = 0;
		_loaded = new bool[4];
		DeathHandled = false;
		_audio_source = GetComponent<AudioSource>();
		
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
			_audio_source.clip = FireSounds[Random.Range(0, FireSounds.Length - 1)];
			_audio_source.pitch = Random.Range(0.95f, 1.05f);
			_audio_source.Play();
		}
        else if (!_loaded[_current_barrallel])
        {
            _audio_source.clip = EmptyClipSound;
            _audio_source.pitch = Random.Range(0.95f, 1.05f);
            _audio_source.Play();
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

		_audio_source.clip = ReloadSound;
		_audio_source.pitch = Random.Range(0.95f, 1.05f);
		_audio_source.Play();

		_loaded[index] = true;
	}

	private Vector2 FindSpawnPoint(int try_num = 0)
	{
		var spawn_points = GameObject.FindGameObjectsWithTag("SpawnPoint");
		var pos = spawn_points[Random.Range(0, spawn_points.Count() - 1)].transform.position;

	    var all_players = GameObject.FindGameObjectsWithTag("Player");

	    if (try_num == 20)
	        return pos;

        foreach (var player in all_players)
	    {
	        if (player == _player_control.gameObject)
                continue;

	        var control = player.GetComponent<PlayerControl>();

	        if (control == null)
	            continue;

	        var distance_to_player = (player.transform.position - pos).magnitude;

	        if (distance_to_player < 1.0f)
                return FindSpawnPoint(try_num++);
	    }

	   return pos;
	}

	public void Update()
	{
		var shoot_pressed = Input.GetAxis(_player_control.GetInputName("Shoot")) > 0.5f;

		if (_player_control.Dead && !DeathHandled)
		{
			_gui.Hide();
			transform.parent = null;
			gameObject.AddComponent<Rigidbody2D>();
			gameObject.AddComponent<BoxCollider2D>();
			DeathHandled = true;
			rigidbody2D.AddForce(new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f))* 200.0f);
			transform.Rotate(new Vector3(0, 0, Random.Range(-25f, 25f)));		
		}

		if (_player_control.Dead)
		{
			if (Time.time > _player_control.TimeOfDeath + 2.0f)
			{
				_player_control.Dead = false;
				var new_arm = (GameObject)Instantiate(ArmPrototype);
				new_arm.transform.parent = _player_control.transform;
				new_arm.transform.localScale = new Vector3(1, 1, 1);
				_player_control.SetArm(new_arm.transform);
				_player_control.transform.position = FindSpawnPoint();
				_player_control.Reset();
				new_arm.GetComponent<ArmControl>()._set_shoot_button_state = true;

				var new_arm_rigidbody = new_arm.rigidbody2D;
				var new_arm_collider = new_arm.collider2D;

				if (new_arm_rigidbody != null)
					Destroy(new_arm_rigidbody);

				if (new_arm_collider != null)
					Destroy(new_arm_collider);
					
				Destroy(gameObject);
			}

			return;
		}

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
		
		if (shoot_pressed && !_fire_held_last_frame && Time.time > _can_shoot_at)
		{
			var clip_empty = ClipEmpty();
			if (_reloading && !clip_empty)
			{
				_current_barrallel = Random.Range(0, 3);
				_reloading = false;
				_gui.Hide();
			}

			if (clip_empty)
			{
				_audio_source.clip = EmptyClipSound;
				_audio_source.pitch = Random.Range(0.95f, 1.05f);
				_audio_source.Play();
			}
			else
				Fire();
		}

		_fire_held_last_frame = shoot_pressed;
	}
}
