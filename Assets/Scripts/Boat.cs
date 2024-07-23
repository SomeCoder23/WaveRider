using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boat : MonoBehaviour
{
    #region Singleton
    public static Boat instance;
    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("More than one Boat!");
            return;
        }

        instance = this;
    }

    #endregion

    public float speed = 5f;
    public float turnSpeed = 2f;
    public float maxTiltAngle = 16f;
    public float speedUpTimer = 30;
    public float speedModifier = 2.4f;
    public Shield shield;

    Rigidbody rb;
    Vector3 movement;
    Vector2 moveInput;
    Vector3 startPos;

    bool speedingUp = false;
    bool start = false;
    float timer = 0;
    float initialSpeed, initialTurnSpeed;
    bool safe = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        initialSpeed = speed;
        initialTurnSpeed = turnSpeed;
    }

    public void StartBoat(bool start = true)
    {
        transform.position = startPos;
        speed = initialSpeed;
        turnSpeed = initialTurnSpeed;
        moveInput = new Vector2(0,0);
        this.start = start;
    }
    

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!start) return;

        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!start) return;
 
        if (!speedingUp)
            movement = transform.forward * speed * Time.fixedDeltaTime;

        movement.x = moveInput.x * -turnSpeed * Time.fixedDeltaTime;

        transform.position += movement;
        float angle = moveInput.x * -maxTiltAngle;
        float tiltAngle = Mathf.LerpAngle(rb.rotation.eulerAngles.z, angle, Time.fixedDeltaTime * turnSpeed /4);
        Quaternion tiltRotation = Quaternion.Euler(0f, 180f, tiltAngle);
        rb.MoveRotation(tiltRotation);

        if (timer <= speedUpTimer)
            timer += Time.fixedDeltaTime;
        else
        {
            speed += speedModifier;
            turnSpeed += (speedModifier / 2f);
            timer = 0;
        }

    }


    public void SpeedUp(float direction)
    {
        speedingUp = true;
        rb.AddForce(new Vector3(0, 0, direction) * speed * Time.fixedDeltaTime);
        //movement.z = direction * speed * Time.fixedDeltaTime;
    }

    public void StopSpeedUp()
    {
        speedingUp = false;
    }


    public void StopBoat()
    {
        start = false;
        transform.rotation = Quaternion.Euler(0, 180f, 0);
        rb.velocity = new Vector3(0, 0, 0);

    }

    public void EnableShield(float time)
    {
        if (shield != null && !safe)
        {
            shield.Activate(time);
            HUD_Manager.instance.ActivateShieldTime(time);
            safe = true;
        }
    }

    public void DisableShield()
    {
        safe = false;
        HUD_Manager.instance.DeactivateShieldTimer();
    }

    public bool isProtected()
    {
        return safe;
    }
}
