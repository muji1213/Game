using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed;
    public float breakForce;
    public float decelation;
    private bool right;
    private bool left;
    private bool up;
    private bool down;

    public AnimationCurve force;
    private float timer;


    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        right = Input.GetKey(KeyCode.RightArrow);
        left = Input.GetKey(KeyCode.LeftArrow);
        up = Input.GetKey(KeyCode.UpArrow);
        down = Input.GetKey(KeyCode.DownArrow);
    }

    private void FixedUpdate()
    {
        float reverseForce;
        if (right && up)
        {
            timer += Time.deltaTime;
            var forcespeed = force.Evaluate(timer);
            if (rb.velocity.x < 0 && rb.velocity.y < 0)
            {
                reverseForce = breakForce;
            }
            else
            {
                reverseForce = 0.7f;
            }
            rb.AddForce(forcespeed * speed * reverseForce, forcespeed * speed * reverseForce, 0);
        }
        else if (right && down)
        {
            timer += Time.deltaTime;
            var forcespeed = force.Evaluate(timer);
            if (rb.velocity.x < 0 && rb.velocity.y > 0)
            {
                reverseForce = breakForce;
            }
            else
            {
                reverseForce = 0.7f;
            }
            rb.AddForce(forcespeed * speed * reverseForce, -forcespeed * speed * reverseForce, 0);
        }
        else if (left && down)
        {
            timer += Time.deltaTime;
            var forcespeed = force.Evaluate(timer);
            if (rb.velocity.x > 0 && rb.velocity.y > 0)
            {
                reverseForce = breakForce;
            }
            else
            {
                reverseForce = 0.7f;
            }
            rb.AddForce(-forcespeed * speed * reverseForce, -forcespeed * speed * reverseForce, 0);
        }
        else if (left && up)
        {
            timer += Time.deltaTime;
            var forcespeed = force.Evaluate(timer);
            if (rb.velocity.x > 0 && rb.velocity.y < 0)
            {
                reverseForce = breakForce;
            }
            else
            {
                reverseForce = 0.7f;
            }
            rb.AddForce(-forcespeed * speed * reverseForce, forcespeed * speed * reverseForce, 0);
        }
        else if (right)
        {
            timer += Time.deltaTime;
            var forcespeed = force.Evaluate(timer);
            if (rb.velocity.x < 0)
            {
                reverseForce = breakForce;
            }
            else
            {
                reverseForce = 1;
            }
            rb.AddForce(Vector3.right * forcespeed * speed * reverseForce);
        }
        else if (left)
        {
            timer += Time.deltaTime;
            var forcespeed = force.Evaluate(timer);
            if (rb.velocity.x > 0)
            {
                reverseForce = breakForce;
            }
            else
            {
                reverseForce = 1;
            }
            rb.AddForce(Vector3.left * forcespeed * speed * reverseForce);
        }
        else if (down)
        {
            timer += Time.deltaTime;
            var forcespeed = force.Evaluate(timer);
            if (rb.velocity.y > 0)
            {
                reverseForce = breakForce;
            }
            else
            {
                reverseForce = 1;
            }
            rb.AddForce(Vector3.down * forcespeed * speed * reverseForce);
        }
        else if (up)
        {
            timer += Time.deltaTime;
            var forcespeed = force.Evaluate(timer);
            if (rb.velocity.y < 0)
            {
                reverseForce = breakForce;
            }
            else
            {
                reverseForce = 1;
            }
            rb.AddForce(Vector3.up * forcespeed * speed * reverseForce);
        }
        else
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                timer = 0.0f;
            }

            if (rb.velocity.x > decelation)
            {
                rb.AddForce(Vector3.left * decelation);
            }
            else if (rb.velocity.x < -decelation)
            {
                rb.AddForce(Vector3.right * decelation);
            }
            else if (rb.velocity.y > decelation)
            {
                rb.AddForce(Vector3.down * decelation);
            }
            else if (rb.velocity.y < -decelation)
            {
                rb.AddForce(Vector3.up * decelation);
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }
        }
    }
}
