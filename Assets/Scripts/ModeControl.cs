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

    public Steering mode;
    public TMP_Text modeText;
    public GameObject  arrivalText;
    public GameObject MainMenuButton;
    Rigidbody2D playerRigidBody;
    [SerializeField] float seekSpeed;
    public GameObject Enemy;

    Rigidbody2D enemyRigidBody;

    void Start()
    {
        arrivalText.gameObject.SetActive(false);

        Enemy.SetActive(false);
        modeText.text = " Angular Seek";

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
            Debug.Log("pressed 2,Arrival Mode");
            modeText.text = " ArrivalMode";
            mode = Steering.ArrivalMode;
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("pressed 2,Obstacle Avoidance");
            modeText.text = " Obstacle Avoidance Mode";
            mode = Steering.ObstacleAvoidance;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            Debug.Log("pressed 2,Reset Mode");
            modeText.text = " Reset Mode";
            mode = Steering.Reset;
        }


        if (mode == Steering.SeekMode)
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

        if (mode == Steering.FleeMode)
        {

            Enemy.SetActive(true);
           
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.z = 0f;

             Enemy.transform.position = mouse;
            Vector3 Enemydirection =playerRigidBody.velocity;
            Enemy.transform.right = Enemydirection;

            Vector2 currentVelocity = playerRigidBody.velocity;
            Vector2 desiredVelocity = (-mouse + transform.position).normalized * seekSpeed;
            Vector2 seekForce = desiredVelocity - currentVelocity;

            playerRigidBody.AddForce(seekForce);
            Vector3 direction = playerRigidBody.velocity;
            transform.right = direction;
        }

        if (mode == Steering.ArrivalMode)
        {
              arrivalText.gameObject.SetActive(true);
            ResetGame();
        }
        else
        {
            arrivalText.gameObject.SetActive(false);
        }


        if(mode == Steering.Reset) 
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
            transform.position = new Vector3(10f,0f,0f);
      
           
            Enemy.transform.position=new Vector3(-5f,0f,0f);    
           
        }
    }
}