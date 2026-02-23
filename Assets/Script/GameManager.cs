using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameManager : MonoBehaviour
{
    [Header("Настройки игры (Можно менять)")]
    [SerializeField] int defaultCoinsLoss;
    [SerializeField] float gameSpeedChange;
    [Header("Настройки скорости (Можно менять)")]
    public float defaultSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float chargeSpeed;
    [SerializeField] float boostTime;
    [SerializeField] Slider powerSlider;
    float speedVelocity; 
    float powerVelocity;
    public float currentSpeed {get; private set;}

    [Header("Состояния (Не трогать)")]
    public bool isBoosting = false;
    bool isGameEnded = false;
    [Header("Progress (Не трогать)")]
    int coinsCollected;
    [SerializeField] TextMeshProUGUI coinsText;
    //[SerializeField] Slider progressSlider;
    //[SerializeField] float gameDuration;
    //float gameTimer;
    [SerializeField] GameObject gameEndScreen;
    [SerializeField] TextMeshProUGUI finalCoinsText;
    [SerializeField] TextMeshProUGUI resultText;
    

    void Awake()
    {
        Ship.ShipDamaged += OnShipDamaged;
        Ship.CoinCollected += OnCoinCollected;
        ShipTop.ShipTopDamaged += OnShipDamaged;
    }
    void OnDestroy()
    {
        Ship.ShipDamaged -= OnShipDamaged;
        Ship.CoinCollected -= OnCoinCollected;
        ShipTop.ShipTopDamaged -= OnShipDamaged;
    }
    void Start()
    {
        Time.timeScale = 1;
        powerSlider.value = 0;
        coinsCollected = 0;
        //gameTimer = gameDuration;
        //progressSlider.value = 0;
        coinsText.text = coinsCollected.ToString();
        powerSlider.gameObject.SetActive(false);
        gameEndScreen.SetActive(false);
        currentSpeed = defaultSpeed;
        isBoosting = false;
        isGameEnded = false;
    }

    void Update()
    {
        //Debug.Log(currentSpeed);
        /*gameTimer -= Time.deltaTime;
        if (gameTimer <= 0){isGameEnded = true;}
        progressSlider.value = 1 - (gameTimer/gameDuration);*/


        IncreaseGameSpeed();
        if (powerSlider.value > 0){
            powerSlider.gameObject.SetActive(true);
        }
        else{
            powerSlider.gameObject.SetActive(false);
        }
        
        if (Input.GetKey(KeyCode.W)&!isBoosting){
            powerSlider.value = Mathf.MoveTowards(powerSlider.value, powerSlider.maxValue, Time.deltaTime*chargeSpeed);
        }

        if (Input.GetKeyUp(KeyCode.W)&&!isBoosting){
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
        if (isGameEnded){
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

    void OnShipDamaged(){
        coinsCollected -= defaultCoinsLoss;
        coinsText.text = coinsCollected.ToString();
        if (coinsCollected <= 0) OnShipDied();
    }
    void OnShipDied()
    {
            Time.timeScale = 0;
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
    void OnCoinCollected()
    {
        coinsCollected++;
        coinsText.text = coinsCollected.ToString();
    }
    void IncreaseGameSpeed()
    {
        float delta = gameSpeedChange * Time.deltaTime;
        defaultSpeed += delta;
        maxSpeed += delta;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
