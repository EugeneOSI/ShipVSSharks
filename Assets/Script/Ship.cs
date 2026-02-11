using System;
using UnityEngine;
public class Ship : MonoBehaviour
{
    [Header("Физика корабля")]
    [SerializeField] private Rigidbody2D _RB;
    [SerializeField] private float rayLength = 10f;
    [SerializeField] private float rideSpringStrength = 100f;
    [SerializeField] private float rideSpringDamper = 10f;
    [SerializeField] private float maxTiltAngle = 30f;
    [SerializeField] private float rideHeight;
    [SerializeField] private LayerMask layerMask;
    [Header("Ship Suspension")]
    [SerializeField] private Transform[] suspensionPoints;

    public static Action ShipDied;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        //transform.position = new Vector2(0, transform.position.y);

        foreach (var point in suspensionPoints){
            Vector2 origin = point.position;
            Vector2 rayDir = Vector2.down;
            RaycastHit2D hit = Physics2D.Raycast(origin, rayDir, rayLength, layerMask);
            Debug.DrawRay(origin, rayDir * rayLength, Color.blue);

            if (hit.collider == null){
                continue;
            }


            Vector2 vel = _RB.GetPointVelocity(origin);
            float rayDirVel = Vector2.Dot(rayDir, vel);

            float x = hit.distance - rideHeight;
            float springForce = (x * rideSpringStrength) - (rayDirVel * rideSpringDamper);

            Vector2 force = rayDir * springForce;
            Debug.DrawRay(origin, rayDir * springForce, Color.yellow);

            _RB.AddForceAtPosition(force, origin);
        }
        float z = _RB.rotation;

        if (z > 180f) z -= 360f;


        float clamped = Mathf.Clamp(z, -maxTiltAngle, maxTiltAngle);

        if (!Mathf.Approximately(z, clamped))
           {
            _RB.MoveRotation(clamped);
            _RB.angularVelocity = 0f;
           }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shark"))
        {
            Debug.Log("Ship Died");
            ShipDied?.Invoke();
        }
    }
}
