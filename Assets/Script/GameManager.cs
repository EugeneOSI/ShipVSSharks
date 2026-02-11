using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [Header("Настройки спавна")]
    [SerializeField] float yPos = 0f;
    public float xLimit = 22f;
    [SerializeField] float waveSpawnDelay = 3f;
    [SerializeField] float sharkSpawnDelay = 6f;
    [SerializeField] float delayDifference = 1f;
    [SerializeField] float yDifference = 0.5f;

    [Header("Префабы")]
    [SerializeField] GameObject waterWavePrefab;
    [SerializeField] GameObject sharkPrefab;
    [Header("Настройки скорости")]
    public float defaultSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float chargeSpeed;
    [SerializeField] float boostTime;
    [SerializeField] Slider powerSlider;
    float speedVelocity; 
    float powerVelocity;
    public float currentSpeed {get; private set;}

    [Header("Состояния")]
    public bool isBoosting = false;
    bool isSpawning = false;

    void Awake()
    {
        Ship.ShipDied += OnShipDied;
    }
    void OnDestroy()
    {
        Ship.ShipDied -= OnShipDied;
    }
    void Start()
    {
        powerSlider.value = 0;
        powerSlider.gameObject.SetActive(false);
        currentSpeed = defaultSpeed;
        isBoosting = false;
        isSpawning = false;
    }

    void Update()
    {
        //Debug.Log(currentSpeed);
        if(!isSpawning) {
            StartCoroutine(SpawnObject(yDifference, delayDifference));
            }

        if (powerSlider.value > 0){
            powerSlider.gameObject.SetActive(true);
        }
        else{
            powerSlider.gameObject.SetActive(false);
        }
        
        if (Input.GetKey(KeyCode.Space)&!isBoosting){
            powerSlider.value = Mathf.MoveTowards(powerSlider.value, powerSlider.maxValue, Time.deltaTime*chargeSpeed);
        }

        if (Input.GetKeyUp(KeyCode.Space)&!isBoosting){
            isBoosting = true;
            currentSpeed = maxSpeed*powerSlider.value;
        }
        if (isBoosting){
            Debug.Log(currentSpeed);
            float newSpeed = Mathf.SmoothDamp(currentSpeed,defaultSpeed,ref speedVelocity,boostTime);
            currentSpeed = newSpeed;
            powerSlider.value = Mathf.SmoothDamp(powerSlider.value, 0, ref powerVelocity,boostTime);
            if (currentSpeed - defaultSpeed < 0.5f){
            currentSpeed = defaultSpeed;
            powerSlider.value = 0;
            isBoosting = false;
            }
        }
        
    }

    void OnShipDied()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator SpawnObject(float yDifference, float delayDifference){
        isSpawning = true;
        Debug.Log("Spawning Water Wave");
        yield return new WaitForSeconds(Random.Range(waveSpawnDelay - delayDifference, waveSpawnDelay + delayDifference));
        Instantiate(waterWavePrefab, new Vector3(xLimit, Random.Range(yPos - yDifference, yPos + yDifference), 0), Quaternion.Euler(0, 0,Random.Range(-20,-45)));
        yield return new WaitForSeconds(Random.Range(sharkSpawnDelay - delayDifference, sharkSpawnDelay + delayDifference));
        Instantiate(sharkPrefab, new Vector3(xLimit, Random.Range(yPos - yDifference, yPos + yDifference), 0), Quaternion.Euler(0, 0,Random.Range(-20,-45)));
        isSpawning = false;
    }
}
