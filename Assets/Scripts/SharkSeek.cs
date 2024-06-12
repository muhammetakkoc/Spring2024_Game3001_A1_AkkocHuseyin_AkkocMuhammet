using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkSeek : MonoBehaviour
{

    Rigidbody2D sharkRigidBody;
    public float sharkSpeed;
    public GameObject smallFish;
    // Start is called before the first frame update
    void Start()
    {
        sharkRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 currentVelocity = sharkRigidBody.velocity;
        Vector2 desiredVelocity = (smallFish.transform.position - transform.position).normalized * sharkSpeed;
        Vector2 seekForce = desiredVelocity - currentVelocity;

        sharkRigidBody.AddForce(seekForce);

        Vector3 direction = sharkRigidBody.velocity;
        transform.right = -direction;
    }
}
