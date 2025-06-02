using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backLeft;
    [SerializeField] WheelCollider backRight;

    [SerializeField] Transform frontRightTransform;
    [SerializeField] Transform frontLeftTransform;
    [SerializeField] Transform backLeftTransform;
    [SerializeField] Transform backRightTransform;


    public float acceleration = 900f;
    public float breakingForce = 850f;
    public float maxTurnAngle = 30f;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    private void FixedUpdate(){

        // get forward/reverse acceleration from the vertical axis (w and s)
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        // if we are pressing space, give currrentBreakingForce a value
        if(Input.GetKey(KeyCode.Space))
            currentBreakForce = breakingForce;
        else
            currentBreakForce = 0f;

        // apply acceleration to front wheels.
         frontRight.motorTorque = currentAcceleration;
         frontLeft.motorTorque = currentAcceleration;

         frontRight.brakeTorque = currentBreakForce;
         frontLeft.brakeTorque = currentBreakForce;
         backLeft.brakeTorque = currentBreakForce;
         backRight.brakeTorque = currentBreakForce;

         // take care of the steering
         currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");

         frontRight.steerAngle = currentTurnAngle;
         frontLeft.steerAngle = currentTurnAngle;

         // update wheel meshes after turning
         UpdateWheel(frontLeft, frontLeftTransform);
         UpdateWheel(frontRight, frontRightTransform);
         UpdateWheel(backLeft, backLeftTransform);
         UpdateWheel(backRight, backRightTransform);
    }

    void UpdateWheel(WheelCollider col, Transform trans){

        // get wheel collider state
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        // set wheel transform state
        trans.position = position;
        trans.rotation = rotation;
    }

}
