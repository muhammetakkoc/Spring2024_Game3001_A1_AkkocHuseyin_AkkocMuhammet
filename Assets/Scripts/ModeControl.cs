using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public enum Steering
{
   SeekMode,
    FleeMode,
    ArrivalMode,
    ObstacleAvoidance,
    Reset
}
public class PlayerMovement : MonoBehaviour
{
    //
    public Transform obstacle;


    //
   // float probeAngel = 30f;
    public Steering mode;
    public TMP_Text modeText;
    SpriteRenderer sr;
    public GameObject MainMenuButton;
    Rigidbody2D playerRigidBody;
    [SerializeField] float seekSpeed;

    public GameObject Enemy;
    float distance;
    public GameObject fishingWorm;

    Rigidbody2D enemyRigidBody;

    void Start()
    {

        Enemy.SetActive(false);
        fishingWorm.SetActive(false);
        modeText.text = " Angular Seek";

        sr = GetComponent<SpriteRenderer>();
        playerRigidBody = GetComponent<Rigidbody2D>();
    }



    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("pressed 1, Angular Seek");
            modeText.text = " Angular Seek";
            mode = Steering.SeekMode;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            Debug.Log("pressed 2,Flee Mode");
            modeText.text = " Flee Mode";
            mode = Steering.FleeMode;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            Debug.Log("pressed 3,Arrival Mode");
            modeText.text = " ArrivalMode";
            mode = Steering.ArrivalMode;
            fishingWorm.SetActive(true);
            Enemy.SetActive(false);
            fishingWorm.transform.position = new Vector2(Random.Range(-13f, 1f), Random.Range(-6f, 12f));
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("pressed 2,Obstacle Avoidance");
            modeText.text = " Obstacle Avoidance Mode";
            mode = Steering.ObstacleAvoidance;
        }


        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            modeText.text = " Blend Mode";

        }



        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            Debug.Log("pressed 2,Reset Mode");
            modeText.text = " Reset Mode";
            mode = Steering.Reset;
        }

        ////////////////////  Seek Mode
        if (mode == Steering.SeekMode)
        {

            Seek();
        }
        ////////////////////////// Flee mode
        if (mode == Steering.FleeMode)
        {
            //GetComponent(Avoid).enabled = false;
            Enemy.SetActive(true);

            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;

            Enemy.transform.position = mouse;
            Vector3 Enemydirection = playerRigidBody.velocity;
            Enemy.transform.right = -Enemydirection;

            Vector2 currentVelocity = playerRigidBody.velocity;
            Vector2 desiredVelocity = (-mouse + transform.position).normalized * seekSpeed;
            Vector2 seekForce = desiredVelocity - currentVelocity;

            playerRigidBody.AddForce(seekForce);
            Vector3 direction = playerRigidBody.velocity;
            transform.right = direction;
        }
        else
        {
            Enemy.SetActive(false);
        }
        //////////////////// Arrival Mode
        ///

        if (mode == Steering.ArrivalMode)
        {

            Arrival();




        }


        if (mode == Steering.ObstacleAvoidance)
        {
            Avoid();
        }
        //////////////// Reset Mode
        if (mode == Steering.Reset)
        {

            ResetGame();
            MainMenuButton.SetActive(true);

        }
        else
        {
            MainMenuButton.SetActive(false);
        }






        void ResetGame()
        {
            transform.position = new Vector3(2f, 0f, 0f);
            transform.rotation = Quaternion.identity;
            Enemy.SetActive(false);
            modeText.text = " Reset Mode";


        }
    }


    void Arrival()
    {

        Vector2 targetPosition = fishingWorm.transform.position;
        Vector2 currentVelocity = playerRigidBody.velocity;
        Vector2 toTarget = targetPosition - (Vector2)transform.position;
        float distance = toTarget.magnitude;

        float slowingDistance = 5f; // The distance at which to start slowing down
        float deceleration = seekSpeed / slowingDistance; // The deceleration factor

        float speed = Mathf.Min(seekSpeed, distance * deceleration);
        Vector2 desiredVelocity = toTarget.normalized * speed;
        Vector2 seekForce = desiredVelocity - currentVelocity;
        modeText.text = " ArrivalMode ,  Speed: " + speed;
        Debug.Log(speed);
        playerRigidBody.AddForce(seekForce);

        Vector3 direction = playerRigidBody.velocity;
        transform.right = direction;

        if (distance <= 0.5f)
        {
            mode = Steering.Reset;
            fishingWorm.gameObject.SetActive(false);
        }

    }


    void Seek()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;



        Vector2 currentVelocity = playerRigidBody.velocity;
        Vector2 desiredVelocity = (mouse - transform.position).normalized * seekSpeed;
        Vector2 seekForce = desiredVelocity - currentVelocity;

        playerRigidBody.AddForce(seekForce);

        Vector3 direction = playerRigidBody.velocity;
        transform.right = direction;
    }

    void Avoid()
    {
        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;

        // Direction FROM ship TO target
        Vector2 currentVelocity = playerRigidBody.velocity;
        Vector2 desiredVelocity = (mouse - transform.position).normalized * seekSpeed;
        Vector2 seekForce = desiredVelocity - currentVelocity;
        
        // transform.right is the ship's direction
        Vector3 leftDirection = Quaternion.Euler(0.0f, 0.0f, seekSpeed) * transform.right;
        Vector3 rightDirection = Quaternion.Euler(0.0f, 0.0f, -seekSpeed) * transform.right;

        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + leftDirection, leftDirection);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + rightDirection, rightDirection);

        if (leftHit.collider != null)
        {
            // Turn right (transform.up * -1) to avoid left obstacle
            desiredVelocity += new Vector2(transform.up.x, transform.up.y) * seekSpeed * -1.0f;
            //Debug.Log(leftHit.collider.gameObject.name);

        }
        else if (rightHit.collider != null)
        {
            // Turn left (transform.up) to avoid right obstacle
            desiredVelocity += new Vector2(transform.up.x, transform.up.y) * seekSpeed;
            //Debug.Log(rightHit.collider.gameObject.name);
        }

        Vector3 direction = playerRigidBody.velocity;
        transform.right = direction;

        playerRigidBody.AddForce(seekForce);

        //    Seek();




        //    float distance = 5.0f;




        //    float dt = Time.deltaTime;

        //    //Quaternion leftRotation = Quaternion.

        //    Vector3 direction = transform.right;
        //    Physics2D.Raycast(transform.position, direction, distance);
        //    Debug.DrawLine(transform.position, transform.position + direction * distance, Color.magenta);
        //    Vector3 left = Quaternion.Euler(0f, 0f, probeAngel) * direction;
        //    Vector3 right = Quaternion.Euler(0f, 0f, -probeAngel) * direction;


        //    //Physics2D.Raycast(transform.position, direction, distance);
        //    Debug.DrawLine(transform.position, transform.position + left * distance, Color.magenta);
        //    Debug.DrawLine(transform.position, transform.position + right * distance, Color.magenta);

        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);
        //    if (hit.collider != null)
        //    {
        //        Debug.Log(hit.collider.name);

        //        transform.Rotate(0f, 0f, 10f * dt);

        //        Vector2 avoidPosition = transform.position + transform.up * 5f;
        //        playerRigidBody.AddForce(playerRigidBody, avoidPosition, seekSpeed);
        //       // playerRigidBody.AddForce(Seek(playerRigidBody, avoidPosition, seekSpeed));
        //        //Quaternion.RotateTowards(transform.rotation, Quaternion.)


        //        transform.right = playerRigidBody.velocity;



        //    }
        //}
    }
}