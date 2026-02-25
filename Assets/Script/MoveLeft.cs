using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed;
    private GameManager gameManager;
    private SpawnManager spawnManager;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        spawnManager = FindFirstObjectByType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = gameManager.currentSpeed;
        transform.position += new Vector3(-1*speed * Time.deltaTime, 0, 0);
        if (transform.position.x < -spawnManager.xLimit)
        {
            Destroy(gameObject);
        }
    }
}
