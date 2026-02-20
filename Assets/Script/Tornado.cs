using UnityEngine;

public class Tornado : MonoBehaviour
{
        
    public float speed;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }
    void Update()
    {
        float currentSpeed = speed+gameManager.currentSpeed;
        transform.position += new Vector3(-1*currentSpeed * Time.deltaTime, 0, 0);
        if (transform.position.x < -gameManager.xLimit)
        {
            Destroy(gameObject);
        }
    }
}
