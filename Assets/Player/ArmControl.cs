using UnityEngine;

public class ArmControl : MonoBehaviour
{
	public GameObject BulletPrototype;
	public float ShootSpeed = 200.0f;
	private PlayerControl _player_control;
	private Transform _bullet_spawn_point;
	public float ShootCooldown = 0.5f;
	private float _can_shoot_at;

	public void Start()
	{
		_player_control = transform.parent.GetComponent<PlayerControl>();
		_bullet_spawn_point = transform.FindChild("BulletSpawnPoint");
		_can_shoot_at = 0;
	}

	public void Update()
	{
		var look = new Vector2(Input.GetAxis(_player_control.GetAxisName("LookX")), Input.GetAxis(_player_control.GetAxisName("LookY")));

		if (look.magnitude > 0.4f)
			transform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg);
			
		var shoot_pressed = Input.GetAxis(_player_control.GetAxisName("Shoot")) > 0.5f;

		if (shoot_pressed && Time.time > _can_shoot_at)
		{
			var bullet = (GameObject)Instantiate(BulletPrototype);
			bullet.transform.position = _bullet_spawn_point.transform.position;
			bullet.layer = 9 + int.Parse(_player_control.PlayerNum) - 1;
			var direction = (new Vector2(_bullet_spawn_point.transform.position.x, _bullet_spawn_point.transform.position.y) - new Vector2(transform.position.x, transform.position.y)).normalized;
			bullet.rigidbody2D.AddForce(direction * ShootSpeed);
			_can_shoot_at = Time.time + ShootCooldown;
		}
	}
}
