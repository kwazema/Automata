using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CharacterMove : MonoBehaviour, IEntity, FloorMessage
{
    
    public static bool checkWithFloor(int mask, int maskToCompare)
    {
        return (mask & maskToCompare) == maskToCompare;
    }

    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    public float speed = 12f;
    [SerializeField]
    private bool flyMode = false;


    private Vector3 velocity;
    private bool isGrounded;

    public float jumpHeight = 3.0f;
    public float jumpCooldown = 4.0f;
    public float jumpCooldownTimer = 0f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [SerializeField]
    SpatialIndex spatial;

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundDistance);

    }

    void IEntity.EAwake()
    {

    }

   
    void IEntity.EUpdate(float delta)
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (isGrounded && velocity.y < 0)
        {
           velocity.y = -2f;
           spatial.getFloorState(transform.position.x, transform.position.z, gameObject);
        }
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();

        if (!flyMode)
            velocity += Physics.gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        jumpCooldownTimer += Time.deltaTime;
    }

    IEnumerator onFire(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public void getFloorInfo(SpatialIndex.FLOOR_STATUS state)
    {
        if (checkWithFloor((int)state, (int)SpatialIndex.FLOOR_STATUS.WATER))
        {
            speed = 24.0f;
        }
        else if(checkWithFloor((int)state, (int)SpatialIndex.FLOOR_STATUS.LAVA))
        {
            speed = 3.0f;
            Jump();
        }
        else
        {
            speed = 12.0f;
        }
    }

    void Jump()
    {
        if (jumpCooldownTimer >= jumpCooldown)
        {
            jumpCooldownTimer = 0;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
    }
}



