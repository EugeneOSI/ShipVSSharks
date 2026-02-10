using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] float yPos = 0f;
    [SerializeField] GameObject waterWavePrefab;
    bool isSpawning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isSpawning) return;
        else StartCoroutine(SpawnWaterWave(3f));
        
    }

    IEnumerator SpawnWaterWave(float delay){
        isSpawning = true;
        yield return new WaitForSeconds(delay);
        Instantiate(waterWavePrefab, new Vector3(10, yPos, 0), Quaternion.identity);
        isSpawning = false;
    }
}
