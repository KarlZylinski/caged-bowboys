using System;
using System.Linq;
using UnityEngine;

namespace Assets.Player
{
	public class Animator : MonoBehaviour
	{
		public Sprite[] RunSprites;
		public Vector2[] RunSpritesArmOffsets;
		public Sprite[] IdleSprites;
		public Vector2[] IdleSpritesArmOffsets;
		public Sprite[] ClimbSprites;
		public Vector2[] ClimbSpritesArmOffsets;
		public bool KillWhenDone = false;

		public float AnimationSpeed = 1 / 10.0f;
		private float _current_time;
		private int _frame_index;
		private SpriteRenderer _sprite_renderer;
		private Sprite[] _current_base_animation;
		private Sprite[] _current_temp_animation;
		private float _temp_anim_speed;
		private float _base_anim_speed;
		private Transform _arm;
		public bool ManualAdvance;
		private bool _advance;

		public void Start()
		{
			_current_time = 0;
			_frame_index = 0;
			_sprite_renderer = GetComponent<SpriteRenderer>();
			_current_base_animation = IdleSprites;
			_current_temp_animation = null;
			_temp_anim_speed = AnimationSpeed;
			_base_anim_speed = AnimationSpeed;
			ManualAdvance = false;
			_advance = false;
			_arm = transform.FindChild("Arm");
		}

		public void Update()
		{
			PlayAnimation(_current_temp_animation != null, _current_temp_animation ?? _current_base_animation, _current_temp_animation != null ? _temp_anim_speed : _base_anim_speed);
		}

		public void Advance()
		{
			_advance = true;
		}

		private void PlayAnimation(bool temp_anim, Sprite[] animation, float speed)
		{
			_current_time += Time.deltaTime;

			if ((!ManualAdvance && _current_time > speed) || (ManualAdvance && _advance))
			{
				_advance = false;
				_current_time = 0;
				++_frame_index;

				if (_frame_index >= animation.Count())
				{
					if (KillWhenDone)
					{
						Destroy(gameObject);
						return;
					}

					_frame_index = 0;

					if (temp_anim)
					{
						_current_temp_animation = null;
						animation = _current_base_animation;
					}
				}
				_arm.transform.localPosition = ArmOffset(animation, _frame_index);
				_sprite_renderer.sprite = animation[_frame_index];
			}
		}

		

		private Vector2 ArmOffset(Sprite[] sprites, int index)
		{
			if (sprites == RunSprites)
				return RunSpritesArmOffsets[index];

			if (sprites == IdleSprites)
				return IdleSpritesArmOffsets[index];

			if (sprites == ClimbSprites)
				return ClimbSpritesArmOffsets[index];

			throw new NotImplementedException();
		}

		public void SetBaseAnimation(Sprite[] sprites, float speed)
		{
			if (!sprites.Any())
				return;


			_base_anim_speed = 1 / 5.0f;
			
			if (_current_base_animation == sprites)
				return;

			_current_base_animation = sprites;
			_frame_index = 0;
			_current_time = 0;
			_arm.transform.localPosition = ArmOffset(sprites, 0);

			if (_current_temp_animation == null && _sprite_renderer != null && _current_base_animation != null)
				_sprite_renderer.sprite = _current_base_animation[_frame_index];
		}

		public void SetTempAnimation(Sprite[] sprites, float speed)
		{
			if (!sprites.Any())
				return;

			_temp_anim_speed = 1 / 5.0f / Mathf.Abs(speed);

			if (_current_temp_animation != null)
				return;

			_current_temp_animation = sprites;
			_frame_index = 0;
			_current_time = 0;
			_sprite_renderer.sprite = _current_temp_animation[_frame_index];
		}
	}
}