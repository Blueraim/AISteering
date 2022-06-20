using UnityEngine;
using DataStructure;
public enum SteeringState { Seek, Flee, arrive }

[RequireComponent(typeof(Rigidbody2D))]
public class SeekAndFlee : MonoBehaviour
{
    private Rigidbody2D character;

    public Transform target;

    public float maxSpeed = 6f;

    public float maxAcceleration = 2f;

    public float arrivalRadius = 0.1f;
    public float slowRadius = 5f;
    public float timeTimeTarget= 0.1f;


    public SteeringState steeringState = SteeringState.Seek;

    private SteeringOutput steeringOutput = new SteeringOutput { acceleration = Vector3.zero, angular = 0f };

    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        character = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(steeringState == SteeringState.arrive){
            getArrivingOutput();
        }
        else{
            getSteeringOutput();
        }

        character.transform.position += velocity * Time.fixedDeltaTime;

        velocity += steeringOutput.acceleration * Time.fixedDeltaTime;

        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }
    }
    
    private void getArrivingOutput()
    {
        Vector3 direction = target.position - character.transform.position;
        float distance = direction.magnitude;

        float targetSpeed = 0;
        if(distance < arrivalRadius)
        {
            steeringOutput.acceleration = Vector3.zero;
            velocity = Vector3.zero;
            return;
        }

        if(distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * distance / slowRadius;
        }

        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        steeringOutput.acceleration = (targetVelocity - velocity) / timeTimeTarget;

        if(steeringOutput.acceleration.magnitude > maxAcceleration)
        {
            steeringOutput.acceleration.Normalize();
            steeringOutput.acceleration *= maxAcceleration;
        }
    }

    private void getSteeringOutput()
    {
        steeringOutput.acceleration = steeringState == SteeringState.Seek ? target.position - character.transform.position : character.transform.position - target.position;
        
        steeringOutput.acceleration = steeringOutput.acceleration.normalized;
        
        steeringOutput.acceleration *= maxAcceleration;
    }
}