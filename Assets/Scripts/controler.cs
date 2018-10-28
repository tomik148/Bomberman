using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class controler : MonoBehaviour {

    public Animator animator;
    public Joystick joystick;
    public Rigidbody2D rigidbody;

    public float speed = 10;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var dx = joystick.m_HorizontalVirtualAxis.GetValue;
        var dy = joystick.m_VerticalVirtualAxis.GetValue;
        var delta = new Vector2();
        if (dx > 0.1 || dx < -0.1)
        {
            animator.SetFloat("Xvel",dx);
            delta.x += dx;
        }
        if (dy > 0.1 || dy < -0.1)
        {
            animator.SetFloat("Yvel",dy);
            delta.y += dy;
        }
        delta *= Time.deltaTime;
        delta *= speed;
        rigidbody.MovePosition(rigidbody.position + delta);
        
	}
}
