using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    

    Rigidbody2D rb;

    //Velocity
    Vector2 velocity = Vector2.zero;

    bool isFacingLeft = false;


    public float maxSpeed = 2;

    bool isInputing = false;
    public float timeSinceInput = 0;

    //Velocity Curve
    public AnimationCurve acceleration;

    public enum FacingDirection
    {
        left, right
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        MovementUpdate();
        
    }

    private void MovementUpdate()
    {

    //--Horizontal Movement

       //Count the timer up or down depending on weather the player is inputting or not 
       if(isInputing)
        { 
            timeSinceInput += Time.deltaTime;
        }
        else
        {
            timeSinceInput -= Time.deltaTime;
        }

        //Update the velocity using my acceleration curve with TimeSinceInput as the independant variable
        if (isFacingLeft) 
        {
            rb.linearVelocityX = acceleration.Evaluate(timeSinceInput) * -maxSpeed;

        }
        else
        {
            rb.linearVelocityX = acceleration.Evaluate(timeSinceInput) * maxSpeed;
        }
        

    }

    public void takeInput(InputAction.CallbackContext context)
    {


        //Save weather the player is currently inputing
        if (context.started)
        {
            isInputing = true;
            
            //Also reset the input timer
            timeSinceInput = 0;

        }
        else if (context.canceled)
        {
            isInputing = false;

            //Cap the input timer
            timeSinceInput = 0.2f;
        }


        //Take input direction and store it locally
        Vector2 inputDir = new Vector2 (context.ReadValue<Vector2>().x, 0);

        //Determine which direction the player now faces
        if (inputDir.x > 0)
        {
            isFacingLeft = false;
        }
        else if (inputDir.x < 0)
        {
            isFacingLeft = true;
        }

    }

    public bool IsWalking()
    {
        return Mathf.Abs(rb.linearVelocityX) > 0;
    }
    public bool IsGrounded()
    {
        
        return rb.Cast(Vector2.down, new RaycastHit2D[1], 0.1f) > 0;
    }

    public FacingDirection GetFacingDirection()
    {
        if (isFacingLeft)
        {
            return FacingDirection.left;
        }
        else 
        { 
            return FacingDirection.right; 
        }
    }
}
