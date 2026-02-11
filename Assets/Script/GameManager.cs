using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    
    [Header("Настройки спавна")]
    [SerializeField] float yPos = 0f;
    public float xLimit = 22f;
    [SerializeField] float waveSpawnDelay = 3f;
    [SerializeField] float sharkSpawnDelay = 6f;
    [SerializeField] float coinSpawnDelay = 1.3f;
    [SerializeField] float delayDifference = 1f;
    [SerializeField] float yDifference = 0.5f;

    [Header("Префабы")]
    [SerializeField] GameObject waterWavePrefab;
    [SerializeField] GameObject sharkPrefab;
    [SerializeField] GameObject coinPrefab;
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
    bool isGameEnded = false;
    bool isSpawning = false;
    [Header("Progress")]
    int coinsCollected;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] Slider progressSlider;
    [SerializeField] float gameDuration;
    float gameTimer;
    [SerializeField] GameObject gameEndScreen;
    [SerializeField] TextMeshProUGUI finalCoinsText;
    [SerializeField] TextMeshProUGUI resultText;
    

    void Awake()
    {
        Ship.ShipDied += OnShipDied;
        Ship.CoinCollected += OnCoinCollected;
    }
    void OnDestroy()
    {
        Ship.ShipDied -= OnShipDied;
        Ship.CoinCollected -= OnCoinCollected;
    }
    void Start()
    {
        powerSlider.value = 0;
        coinsCollected = 0;
        gameTimer = gameDuration;
        progressSlider.value = 0;
        coinsText.text = coinsCollected.ToString();
        powerSlider.gameObject.SetActive(false);
        gameEndScreen.SetActive(false);
        currentSpeed = defaultSpeed;
        isBoosting = false;
        isSpawning = false;
        isGameEnded = false;
    }

    void Update()
    {
        //Debug.Log(currentSpeed);
        gameTimer -= Time.deltaTime;
        if (gameTimer <= 0){isGameEnded = true;}
        progressSlider.value = 1 - (gameTimer/gameDuration);
        
        if(!isSpawning&&!isGameEnded) {
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

        if (Input.GetKeyUp(KeyCode.Space)&&!isBoosting){
            isBoosting = true;
            currentSpeed = maxSpeed*powerSlider.value;
        }
        if (isBoosting){
            //Debug.Log(currentSpeed);
            float newSpeed = Mathf.SmoothDamp(currentSpeed,defaultSpeed,ref speedVelocity,boostTime);
            currentSpeed = newSpeed;
            powerSlider.value = Mathf.SmoothDamp(powerSlider.value, 0, ref powerVelocity,boostTime);
            if (currentSpeed - defaultSpeed < 0.5f){
            currentSpeed = defaultSpeed;
            powerSlider.value = 0;
            isBoosting = false;
            }
        }
        if (isGameEnded&&!isSpawning){
            gameEndScreen.SetActive(true);
            finalCoinsText.text = coinsCollected.ToString();
            if (coinsCollected > 10){
            resultText.text = "Ебать красавчик";
            resultText.color = Color.green;
            }
            else{
            resultText.text = "Ты лох";
            resultText.color = Color.red;
            }
        }
        
    }

    void OnShipDied()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void OnCoinCollected()
    {
        coinsCollected++;
        coinsText.text = coinsCollected.ToString();
    }

    IEnumerator SpawnObject(float yDifference, float delayDifference){
        isSpawning = true;
        Debug.Log("Spawning Water Wave");
        yield return new WaitForSeconds(Random.Range(waveSpawnDelay - delayDifference, waveSpawnDelay + delayDifference));
        Instantiate(waterWavePrefab, new Vector3(xLimit, Random.Range(yPos - yDifference, yPos + yDifference), 0), Quaternion.Euler(0, 0,Random.Range(-30,-45)));
        yield return new WaitForSeconds(Random.Range(sharkSpawnDelay - delayDifference, sharkSpawnDelay + delayDifference));
        Instantiate(sharkPrefab, new Vector3(xLimit, Random.Range(yPos - yDifference, yPos + yDifference), 0), Quaternion.Euler(0, 0,Random.Range(-30,-45)));
        yield return new WaitForSeconds(Random.Range(coinSpawnDelay - delayDifference, coinSpawnDelay + delayDifference));
        Instantiate(coinPrefab, new Vector3(xLimit, Random.Range(yPos+5f - yDifference, yPos + 5f + yDifference), 0), Quaternion.identity);
        isSpawning = false;
    }
}
