using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour
{


    public GameObject smallFish;
    Rigidbody2D sharkRigidBoy;
    public float seekSpeed;
    // Start is called before the first frame update
    void Start()
    {
        sharkRigidBoy = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector2 currentVelocity = sharkRigidBoy.velocity;
        Vector2 desiredVelocity = (transform.position +smallFish.transform.position).normalized * seekSpeed;
        Vector2 seekForce = desiredVelocity - currentVelocity;

        sharkRigidBoy.AddForce(seekForce);
        Vector3 direction = sharkRigidBoy.velocity;
        transform.right = direction;
    }
}
