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
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        if (!isInputing && timeSinceInput >=0)
        {

            timeSinceInput =- Time.deltaTime;

        }
        else if (timeSinceInput <= 0.2) {

            timeSinceInput =+ Time.deltaTime;

        }
        


        MovementUpdate(velocity);
    }

    private void MovementUpdate(Vector2 v)
    {
        rb.AddForce(v);

        
    }

    public void takeInput(InputAction.CallbackContext context)
    {
        //Take input and store it locally
        Vector2 inputDir = new Vector2 (context.ReadValue<Vector2>().x, 0);


        if (inputDir.x > 0)
        {
            isFacingLeft = false;
        }
        else if (inputDir.x < 0)
        {
            isFacingLeft = true;
        }


        velocity = inputDir * acceleration.Evaluate(0.2f) * maxSpeed;

        if (context.started) {

            isInputing = true;

        }
        else if (context.canceled) { 

            isInputing = false;

        }

    }

    public bool IsWalking()
    {
        return false;
    }
    public bool IsGrounded()
    {
        return false;
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
