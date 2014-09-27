using UnityEngine;
using System.Collections;
using Animator = Assets.Player.Animator;

public class HeadScript : MonoBehaviour {
	private Collider2D _trigger;
	public GameObject BloodPrototype;
	private PlayerControl _player_control;
	private Animator _animator;

	void Start ()
	{
		_player_control = transform.parent.GetComponent<PlayerControl>();
		_animator = transform.parent.GetComponent<Animator>();
		_trigger = GetComponent<Collider2D>();
		gameObject.layer = 18 + int.Parse(_player_control.PlayerNum) - 1;
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Bullet")
		{
			var bullet_direction = other.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
			
			for (var i = 0; i < 10; ++i)
			{
				var instance = (GameObject)Instantiate(BloodPrototype);
				instance.transform.position = new Vector3(other.transform.position.x + bullet_direction.x * 0.1f, other.transform.position.y + bullet_direction.y * 0.1f, 0);

				instance.GetComponent<Rigidbody2D>().AddForce((new Vector2(bullet_direction.x, bullet_direction.y * Random.Range(0.25f, 2f)).normalized * 10.0f));
			}

			_player_control.Dead = true;
			_player_control.TimeOfDeath = Time.time;
			_player_control.rigidbody2D.velocity = Vector2.zero;
			_animator.SetDeathAnim();

			Destroy(other.gameObject);
		}
	}
}
