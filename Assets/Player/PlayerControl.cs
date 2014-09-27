using System;
using UnityEngine;
using Animator = Assets.Player.Animator;

public class PlayerControl : MonoBehaviour
{
    public string PlayerNum = "1";
    public float JumpForce = 50.0f;
    public float PlayerMaxSpeed = 3.0f;
	public float ClimbDistance = 50.0f;
    private Transform _grounded_check_box;
    private bool _grounded;
    private string _player_name;
	private Animator _animator;
	private bool _climbing;
	private Vector2 _climbed_since_last_frame;
	private Transform _left_check_box;
	private Transform _right_check_box;
	private Transform _head_check_box;
	private Transform _arm;

	public void Start()
	{
		_arm = transform.FindChild("Arm");
	    _animator = GetComponent<Animator>();
		_grounded_check_box = transform.FindChild("GroundCheck");
		_left_check_box = transform.FindChild("LeftCheck");
		_right_check_box = transform.FindChild("RightCheck");
		_head_check_box = transform.FindChild("HeadCheck");
        _grounded = false;
	    _climbing = false;
        _player_name = "Player" + PlayerNum;
    }

	private bool HitFrom(Transform check_obj, string additional_mask = null)
	{
		var v = check_obj.position - transform.position;
		var dir = v.normalized;
		var distance = v.magnitude;
		var mask =  1 << LayerMask.NameToLayer("Ground");

		if (additional_mask != null)
			mask = mask | 1 << LayerMask.NameToLayer(additional_mask);

		return Physics2D.CircleCast(transform.position + dir * 0.1f, 0.1f, dir, 0.07f, mask).collider != null;
	}

    public void Update()
    {
		_grounded = HitFrom(_grounded_check_box) && rigidbody2D.velocity.y >= 0;
        var movement = new Vector2(Input.GetAxis(GetInputName("MovementX")), Input.GetAxis(GetInputName("MovementY"))) ;
        var jump_held = Input.GetAxis(GetInputName("Jump")) > 0.5f;
	    var climb_held = Input.GetButton(GetInputName("Climb"));
		
	    if (climb_held)
	    {
			_animator.SetBaseAnimation(_animator.ClimbSprites, 0);
		    _grounded = false;
		    _climbing = true;
		    rigidbody2D.velocity = Vector2.zero;
		    rigidbody2D.isKinematic = true;

		    var climb_movement = movement;

			if (HitFrom(_left_check_box, "NoClimb"))
				climb_movement.x = Mathf.Max(climb_movement.x, 0);

			if (HitFrom(_right_check_box, "NoClimb"))
				climb_movement.x = Mathf.Min(climb_movement.x, 0);

			if (HitFrom(_grounded_check_box, "NoClimb"))
				climb_movement.y = Mathf.Max(climb_movement.y, 0);

			if (HitFrom(_head_check_box, "NoClimb"))
				climb_movement.y = Mathf.Min(climb_movement.y, 0);

			rigidbody2D.velocity = climb_movement;
			_climbed_since_last_frame += climb_movement;

		    if (_climbed_since_last_frame.magnitude >= ClimbDistance)
		    {
				_animator.Advance();
			    _climbed_since_last_frame = Vector2.zero;
		    }

		    _animator.ManualAdvance = true;
	    }
        else if (jump_held && _grounded)
        {
            rigidbody2D.AddForce(new Vector3(0, JumpForce, 0), ForceMode2D.Impulse);
            _grounded = false;
        }

	    if (!climb_held && _climbing)
	    {
		    _climbed_since_last_frame = Vector2.zero;
			_animator.ManualAdvance = false;
		    _climbing = false;
		    rigidbody2D.isKinematic = false;
		    rigidbody2D.velocity = Vector2.zero;
	    }

        var limited_speed = LimitSpeed(movement.x, _grounded, rigidbody2D.velocity);

        if (limited_speed == rigidbody2D.velocity)
			rigidbody2D.AddForce(CalculateForceToAdd(movement.x, _grounded));
        else
            rigidbody2D.velocity = limited_speed;

		rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -PlayerMaxSpeed, PlayerMaxSpeed), rigidbody2D.velocity.y);

	    if (!_climbing)
	    {
		    if (Mathf.Abs(rigidbody2D.velocity.x) > 0.2f)
			    _animator.SetBaseAnimation(_animator.RunSprites, 400);
		    else
			    _animator.SetBaseAnimation(_animator.IdleSprites, 400);
	    }

	    if (_climbing)
	    {
			transform.localScale = new Vector3(1, 1, 1);
		    var arm_pos = _arm.transform.position;
			_arm.transform.position = new Vector3(arm_pos.x, arm_pos.y, 1.0f);
	    }
	    else
	    {
			var arm_pos = _arm.transform.position;
			_arm.transform.position = new Vector3(arm_pos.x, arm_pos.y, -1.0f);
			if (rigidbody2D.velocity.x > 0.1f)
				transform.localScale = new Vector3(1, 1, 1);
			else if (rigidbody2D.velocity.x < -0.1f)
				transform.localScale = new Vector3(-1, 1, 1);
	    }
    }

    public string GetInputName(string axis)
    {
        return _player_name + "_" + axis;
    }

    private static Vector3 CalculateForceToAdd(float movement, bool grounded)
    {
        return new Vector3(movement * 50.0f * (grounded ? 1.0f : 0.2f), 0, 0);
    }

    private static Vector2 LimitSpeed(float movement, bool grounded, Vector2 velocity)
    {
        if (Math.Abs(movement) < float.Epsilon && grounded)
            return new Vector2(velocity.x * 0.8f, velocity.y);

        if (Math.Abs(movement) < float.Epsilon)
            return new Vector2(velocity.x * 0.9f, velocity.y);

        return velocity;
    }

	public string PlayerName()
	{
		return _player_name;
	}
}
