using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Ship : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ShipControlType _shipControlType;
    [SerializeField] private InputActionAsset _playerInput;
    private InputActionMap _playerActions;
    [SerializeField] private Rigidbody2D _RB;
    [Header("Скорость корабля")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveAcceleration = 100f;
    [SerializeField] private float maxSpeed = 10f;
    [Header("Поведение корабля в полете")]
    [SerializeField] private float downForce = 50f;
    [SerializeField] private float extraGravity = 100f;
    [SerializeField] private float flyAcceleration = 100f;
    [SerializeField] private float maxFlySpeed = 10f;
    [SerializeField] private float normalGravity;
    bool isFlying;
    bool onWater;

    [Header("Поведение корабля на воде")]
    [SerializeField] private float rayLength = 10f;
    [SerializeField] private float rideSpringStrength = 100f;
    [SerializeField] private float rideSpringDamper = 10f;
    [SerializeField] private float maxTiltAngle = 30f;
    [SerializeField] private float rideHeight;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private bool customSuspention = false;
    [Header("Ship Suspension")]
    [SerializeField] private Transform[] suspensionPoints;
    [Header("Ship Cannon")]
    public float _cannonRecoil;
    [SerializeField] Transform _cannonPosition;
    [SerializeField] ParticleSystem _cannonParticles;
    [SerializeField] GameObject cannonBall;

    public static Action ShipDied;
    public static Action CoinCollected;

    Vector3 initialPosition;
    Quaternion initialRotation;

    void OnEnable(){
        _playerActions.Enable();
    }
    void OnDisable(){
        _playerActions.Disable();
    }
    
    void Awake(){
        _playerInput.Enable();
        _playerActions = _playerInput.FindActionMap("Player");
        _cannonParticles.Stop();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        _RB.gravityScale = normalGravity;
    }


    // Update is called once per frame
    void Update()
    {
        bool isShooting = _playerActions.FindAction("Jump").WasPressedThisFrame();
        if (isShooting)
        {
            Shoot();
            isShooting = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
        if (_shipControlType.currentControlType == ShipControl.tinyWings)
        {
            SurfInputHandler();
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, rayLength, layerMask);
        if (hit.collider != null){
            onWater = true;
            //print("On Water");
        }
        else{
            onWater = false;
            //print("Not On Water");
        }
        Debug.DrawRay(transform.position, -transform.up * rayLength, Color.red);
    }
    void FixedUpdate()
    {

        if (customSuspention){
        SuspentionHandler();}
        
        if (_shipControlType.currentControlType == ShipControl.withMovement)
        {
            UpdateMovement();
        }
        if (_shipControlType.currentControlType == ShipControl.tinyWings)
        {
            SurfHandler();
        }
        
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shark"))
        {
            Debug.Log("Ship Died");
            ShipDied?.Invoke();
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            CoinCollected?.Invoke();
            Destroy(other.gameObject);
        }
    }
    void SuspentionHandler()
    {
        
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
        
    }

    private void SurfInputHandler(){
        if (_playerActions.FindAction("Attack").WasPressedThisFrame()){
            isFlying = true;
        }
        if (_playerActions.FindAction("Attack").WasReleasedThisFrame()){
            isFlying = false;
        }
    }
    private void SurfHandler(){
        if (isFlying&&!onWater){
            _RB.gravityScale = extraGravity;

            if (!onWater){
            _RB.AddForce(Vector2.down  * downForce * Time.fixedDeltaTime, ForceMode2D.Force);}
            print("Flying");
        }
        if (isFlying&&onWater){
            _RB.gravityScale = normalGravity;
            _RB.AddForce(Vector2.right * flyAcceleration * Time.fixedDeltaTime, ForceMode2D.Force);
            print("Surfing");
        }
        else{
            _RB.gravityScale = normalGravity;
            print("Not Flying");
        }
        _RB.linearVelocity = _RB.linearVelocity.magnitude > maxFlySpeed ? _RB.linearVelocity.normalized * maxFlySpeed : _RB.linearVelocity;
        

    }
    
    private void UpdateMovement(){
        Vector2 moveInput = _playerActions.FindAction("Move").ReadValue<Vector2>();
        
        Vector2 direction = new Vector2(moveInput.x, 0f);
        _RB.AddForce(direction * moveSpeed * moveAcceleration * Time.fixedDeltaTime, ForceMode2D.Force);

        _RB.linearVelocity = _RB.linearVelocity.magnitude > maxSpeed ? _RB.linearVelocity.normalized * maxSpeed : _RB.linearVelocity;
    }

    void Shoot(){
        Instantiate(cannonBall, _cannonPosition.position, _cannonPosition.rotation);
        _cannonParticles.Play();
        Vector2 recoilForce = (_cannonPosition.up -_cannonPosition.right) * _cannonRecoil;
        _RB.AddForceAtPosition(recoilForce, _cannonPosition.position);
    }

    void ResetPosition(){
        _RB.linearVelocity = Vector2.zero;
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

}
