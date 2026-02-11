using UnityEngine;

public class WaterWave : MonoBehaviour
{
    private float speed;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = gameManager.currentSpeed;
        transform.position += new Vector3(-1*speed * Time.deltaTime, 0, 0);
        if (transform.position.x < -gameManager.xLimit)
        {
            Destroy(gameObject);
        }
    }
}
