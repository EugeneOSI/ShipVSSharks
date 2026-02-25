using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Ship : MonoBehaviour
{
    [Header("Сила торнадо и отдача пушки (можно менять)")]
    [SerializeField] private float tornadoForce = 100f;
    public float _cannonRecoil;

    [Header("Components")]
    [SerializeField] private ShipControlType _shipControlType;
    [SerializeField] private InputActionAsset _playerInput;
    private InputActionMap _playerActions;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private Rigidbody2D _RB;
    
    [Header("Состояния")]   
    bool onWater;
    bool onTornado;

    [Header("Физика коробля на воде (не трогать)")]
    [SerializeField] private float rayLength = 10f;
    [SerializeField] private float rideSpringStrength = 100f;
    [SerializeField] private float rideSpringDamper = 10f;
    [SerializeField] private float maxTiltAngle = 30f;
    [SerializeField] private float rideHeight;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private bool customSuspention = false;
    [Header("Ship Suspension (не трогать)")]
    [SerializeField] private Transform[] suspensionPoints;
    [Header("Пушка корабля (не трогать)")]
    [SerializeField] Transform _cannonPosition;
    [SerializeField] ParticleSystem _cannonParticles;
    [SerializeField] GameObject cannonBall;

    public static Action ShipDamaged;
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
        
        ShipTop.ShipTopDamaged += OnShipTopDamaged;
        _playerInput.Enable();
        _playerActions = _playerInput.FindActionMap("Player");
        _cannonParticles.Stop();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }
    void OnDestroy(){
        ShipTop.ShipTopDamaged -= OnShipTopDamaged;
    }


    // Update is called once per frame
    void Update()
    {
        bool isShooting = _playerActions.FindAction("Jump").WasPressedThisFrame();
        if (isShooting)
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetPosition();
        }
        Debug.DrawRay(transform.position, -transform.up * rayLength, Color.red);
    }
    void FixedUpdate()
    {
        
        SuspentionHandler();
        if (onTornado){
            _RB.AddForce(Vector2.up * tornadoForce * Time.fixedDeltaTime, ForceMode2D.Force);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shark")|other.gameObject.CompareTag("Ship"))
        {
            ShipDamaged?.Invoke();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            CoinCollected?.Invoke();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Finish")){
            spawnManager.SpawnPattern();
        }
        if (other.gameObject.CompareTag("Tornado")){
            print("On Tornado");
            onTornado = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tornado")){
            onTornado = false;
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

    void OnShipTopDamaged(){
        ResetPosition();
    }

}
