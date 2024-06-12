using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoid : MonoBehaviour
{
    float speed;
    Rigidbody2D rb;
    float probeAngel = 30f;

    SpriteRenderer sr;
    Vector3 SeekAvoid(Rigidbody2D rb, Vector2 mouse, float seekSpeed)
    {

        Vector2 currentVelocity = rb.velocity;
        Vector2 desiredVelocity = (mouse - rb.position).normalized * seekSpeed;
        return desiredVelocity - currentVelocity;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

    }


    void Update()
    {
        float distance = 5.0f;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        rb.AddForce(SeekAvoid(rb, mouse, 5.0f));

        float dt = Time.deltaTime;
        
        //Quaternion leftRotation = Quaternion.

        Vector3 direction = transform.right;
Physics2D.Raycast(transform.position, direction, distance);
Debug.DrawLine(transform.position, transform.position + direction * distance, Color.magenta);
Vector3 left = Quaternion.Euler(0f, 0f, probeAngel) * direction;
Vector3 right = Quaternion.Euler(0f, 0f, -probeAngel) * direction;


//Physics2D.Raycast(transform.position, direction, distance);
Debug.DrawLine(transform.position, transform.position + left * distance, Color.magenta);
Debug.DrawLine(transform.position, transform.position + right * distance, Color.magenta);

RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);
if (hit.collider != null)
{
    Debug.Log(hit.collider.name);

    transform.Rotate(0f, 0f, 10f * dt);

    Vector2 avoidPosition = transform.position + transform.up * 5f;
    rb.AddForce(SeekAvoid(rb, avoidPosition, speed));
    //Quaternion.RotateTowards(transform.rotation, Quaternion.)


            transform.right = rb.velocity;



}
    }
}
