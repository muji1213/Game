using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("X最大速度")] [SerializeField] float maxXspeed;
    [Header("Y最大速度")] [SerializeField] float maxYspeed;
    public float speed;
    public float breakForce;
    public float deceleration;
    public float decelerationMutply;
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

        //X速度が最大値を超えていた場合、X最大速度にする
        if (rb.velocity.x > maxXspeed)
        {
            rb.velocity = new Vector3(maxXspeed, rb.velocity.y, 0);
        }
        else if (rb.velocity.x < -maxXspeed)
        {
            rb.velocity = new Vector3(-maxXspeed, rb.velocity.y, 0);
        }

        //Y速度が最大値を超えていた場合、Y最大速度にする
        if (rb.velocity.y > maxYspeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYspeed, 0);
        }
        else if (rb.velocity.y < -maxYspeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, -maxYspeed, 0);
        }
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

            if (rb.velocity.x > deceleration && rb.velocity.y > deceleration)
            {
                rb.AddForce(-decelerationMutply, -decelerationMutply, 0);
            }
            else if (rb.velocity.x > deceleration && rb.velocity.y < -deceleration)
            {
                rb.AddForce(-decelerationMutply, decelerationMutply, 0);
            }
            else if (rb.velocity.x < -deceleration && rb.velocity.y > deceleration)
            {
                rb.AddForce(decelerationMutply, -decelerationMutply, 0);
            }
            else if (rb.velocity.x < -deceleration && rb.velocity.y < -deceleration)
            {
                rb.AddForce(decelerationMutply, decelerationMutply, 0);
            }

            else if (rb.velocity.x > deceleration)
            {
                rb.AddForce(Vector3.left * decelerationMutply);
            }
            else if (rb.velocity.x < -deceleration)
            {
                rb.AddForce(Vector3.right * decelerationMutply);
            }
            else if (rb.velocity.y > deceleration)
            {
                rb.AddForce(Vector3.down * decelerationMutply);
            }
            else if (rb.velocity.y < -deceleration)
            {
                rb.AddForce(Vector3.up * decelerationMutply);
            }
            else
            {
                Debug.Log("停止中");
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }
        }
    }
}
