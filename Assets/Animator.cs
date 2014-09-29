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
		public Sprite[] IdleDeathSprites;
        public Sprite[] RunDeathSprites;
        public Sprite[] RunDeathSprites2;
        public Sprite[] ClimbDeathSprites;
        public Sprite[] ClimbDeathSprites2;
		public bool StopWhenDone = false;
	    public bool RandomStart = false;

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
		private bool _stopped;
		private bool _hide_when_done;

		public void Start()
		{
			_stopped = false;
			_current_time = RandomStart ? UnityEngine.Random.Range(0.0f, AnimationSpeed) : 0;
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

		public void Reset()
		{
			_stopped = false;
			_current_time = 0;
			_frame_index = 0;
			ManualAdvance = false;
			_advance = false;
			SetBaseAnimation(IdleSprites, 200);
			renderer.enabled = true;
			_hide_when_done = false;
			StopWhenDone = false;
		}


		public void Update()
		{
			PlayAnimation(_current_temp_animation != null, _current_temp_animation ?? _current_base_animation, _current_temp_animation != null ? _temp_anim_speed : _base_anim_speed);
		}

		public void Advance()
		{
			_advance = true;
		}

		public void SetArm(Transform arm)
		{
			_arm = arm;
		}

		private void PlayAnimation(bool temp_anim, Sprite[] animation, float speed)
		{
			if (_stopped)
				return;

			_current_time += Time.deltaTime;

			if ((!ManualAdvance && _current_time > speed) || (ManualAdvance && _advance))
			{
				_advance = false;
				_current_time = 0;
				++_frame_index;

				if (_frame_index >= animation.Count())
				{
					if (!StopWhenDone || _hide_when_done)
					{
						_frame_index = 0;

						if (temp_anim)
						{
							_current_temp_animation = null;
							animation = _current_base_animation;
						}
					}
					
					if(StopWhenDone)
						_stopped = true;

					if (_hide_when_done)
						renderer.enabled = false;
				}

				var arm = _arm != null ? _arm.GetComponent<ArmControl>() : null;
				if (arm != null && !arm.DeathHandled)
				{
					var offset = ArmOffset(animation, _frame_index);

					if (offset != Vector2.zero)
						_arm.transform.localPosition = offset;
				}

				if (!_stopped)
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

			return Vector2.zero;
		}

		public void SetBaseAnimation(Sprite[] sprites, float speed)
		{
			if (!sprites.Any())
				return;


			_base_anim_speed = speed/1000.0f;
			
			if (_current_base_animation == sprites)
				return;

			_current_base_animation = sprites;
			_frame_index = 0;
			_current_time = 0;

			var arm = _arm != null ? _arm.GetComponent<ArmControl>() : null;
			if (arm != null && !arm.DeathHandled)
			{
				var offset = ArmOffset(sprites, 0);

				if (offset != Vector2.zero)
					_arm.transform.localPosition = offset;
			}

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

		public void SetDeathAnim()
		{
			if (_current_base_animation.SequenceEqual(IdleSprites))
				SetBaseAnimation(IdleDeathSprites, 400);

			if (_current_base_animation.SequenceEqual(RunSprites))
                SetBaseAnimation(UnityEngine.Random.Range(1, 3) == 1 ? RunDeathSprites2 : RunDeathSprites, 100);

			if (_current_base_animation.SequenceEqual(ClimbSprites))
			{
				SetBaseAnimation(UnityEngine.Random.Range(1, 2) == 1 ? ClimbDeathSprites : ClimbDeathSprites2, 200);
				_hide_when_done = true;
			}
				

			ManualAdvance = false;
			StopWhenDone = true;
		}
	}
}