using System;
using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    public float JumpForce = 50.0f;
    private Transform _groundedCheckBox;
    private bool _grounded;

    public void Start()
    {
        _groundedCheckBox = transform.Find("GroundCheck");
        _grounded = false;
    }

    private bool ShouldJump()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) && _grounded;
    }

    // Update is called once per frame
    public void Update()
    {
        _grounded = Physics2D.Linecast(transform.position, _groundedCheckBox.position, 1 << LayerMask.NameToLayer("Ground")).collider != null && rigidbody2D.velocity.y >= 0;
        var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (ShouldJump())
        {
            rigidbody2D.AddForce(new Vector3(0, JumpForce, 0));
            _grounded = false;
        }

        rigidbody2D.AddForce(CalculateForceToAdd(input, _grounded));
        rigidbody2D.velocity = LimitSpeed(input, _grounded, rigidbody2D.velocity);
    }

    private static Vector3 CalculateForceToAdd(Vector2 input, bool grounded)
    {    
        return new Vector3(input.x * 50.0f * (grounded ? 1.0f : 0.2f), 0, 0);
    }

    private static Vector2 LimitSpeed(Vector2 input, bool grounded, Vector2 velocity)
    {
        if (Math.Abs(input.x) < float.Epsilon && grounded)
            return new Vector2(0, velocity.y);
        
        if (Math.Abs(input.x) < float.Epsilon)
            return new Vector2(velocity.x * 0.9f, velocity.y);
       
        return new Vector2(Mathf.Clamp(velocity.x, -50, 50), velocity.y);
    }
}
