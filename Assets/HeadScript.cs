using UnityEngine;
using System.Collections;
using Animator = Assets.Player.Animator;

public class HeadScript : MonoBehaviour {
	private Collider2D _trigger;
	public GameObject BloodPrototype;
	private PlayerControl _player_control;
	private Animator _animator;
	public AudioClip[] SplatSounds;
	public AudioClip[] GroanSounds;
	private AudioSource _audio_source;

	void Start ()
	{
		_audio_source = GetComponent<AudioSource>();
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
			
			if (Random.Range(1, 4) == 3)
			{
				_audio_source.clip = GroanSounds[Random.Range(0, GroanSounds.Length - 1)];
				_audio_source.pitch = Random.Range(0.95f, 1.05f);
				_audio_source.Play();	
			}
			else
			{
				_audio_source.clip = SplatSounds[Random.Range(0, SplatSounds.Length - 1)];
				_audio_source.pitch = Random.Range(0.95f, 1.05f);
				_audio_source.Play();
			}

			if (_player_control.Dead)
				return;

			_player_control.Dead = true;
			_player_control.TimeOfDeath = Time.time;
			_player_control.rigidbody2D.velocity = Vector2.zero;

			var score = GameObject.Find("Score").GetComponent<ScoreKeeper>().Score;
			score[other.gameObject.layer - 13]++;

			_animator.SetDeathAnim();

			Destroy(other.gameObject);
		}
	}
}
