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

    }
}
