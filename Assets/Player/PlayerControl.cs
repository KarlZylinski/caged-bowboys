using System;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public string PlayerNum = "1";
    public float JumpForce = 50.0f;
    public float PlayerMaxSpeed = 3.0f;
    private Transform _grounded_check_box;
    private bool _grounded;
    private string _player_name;

    public void Start()
    {
        _grounded_check_box = transform.FindChild("GroundCheck");
        _grounded = false;
        _player_name = "Player" + PlayerNum;
    }

    public void Update()
    {
        _grounded = Physics2D.Linecast(transform.position, _grounded_check_box.position, 1 << LayerMask.NameToLayer("Ground")).collider != null && rigidbody2D.velocity.y >= 0;
        var movement = Input.GetAxis(GetAxisName("Movement"));
        var jump_pressed = Input.GetAxis(GetAxisName("Jump")) > 0.5f;

        if (jump_pressed && _grounded)
        {
            rigidbody2D.AddForce(new Vector3(0, JumpForce, 0), ForceMode2D.Impulse);
            _grounded = false;
        }

        var limited_speed = LimitSpeed(movement, _grounded, rigidbody2D.velocity);

        if (limited_speed == rigidbody2D.velocity)
            rigidbody2D.AddForce(CalculateForceToAdd(movement, _grounded));
        else
            rigidbody2D.velocity = limited_speed;

        rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -PlayerMaxSpeed, PlayerMaxSpeed), rigidbody2D.velocity.y);

		Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }

    public string GetAxisName(string axis)
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
