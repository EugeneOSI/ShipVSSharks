using UnityEngine;
using UnityEngine.U2D;
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
public class OceanGenerator : MonoBehaviour
{
    [Header("Настройки формы")]
    [Range(10, 500)] public int segments = 100;
    public float segmentWidth = 0.5f;
    
    [Header("Настройки волны")]
    public float noiseFrequency = 0.2f; // Насколько часто идут холмы
    public float noiseAmplitude = 5f;   // Насколько они высокие
    public float noiseOffset = 0f;      // Смещение (для имитации движения)

    private EdgeCollider2D _edgeCollider;
    private LineRenderer _lineRenderer;

    void OnValidate() 
    {
        // Метод OnValidate вызывается каждый раз, когда вы меняете значения в инспекторе
        UpdateTerrain();
    }

    void Update()
    {
        // Если мы хотим видеть изменения в реальном времени при изменении Offset в коде
        if (!Application.isPlaying) 
        {
            UpdateTerrain();
        }
    }

    public void UpdateTerrain()
    {
        if (_edgeCollider == null) _edgeCollider = GetComponent<EdgeCollider2D>();
        if (_lineRenderer == null) _lineRenderer = GetComponent<LineRenderer>();

        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < segments; i++)
        {
            float x = i * segmentWidth;
            
            // Используем Mathf.PerlinNoise для плавных, но разнообразных холмов
            // Perlin Noise возвращает значение от 0 до 1, поэтому вычитаем 0.5 для баланса
            float sampleX = (x * noiseFrequency) + noiseOffset;
            float y = (Mathf.PerlinNoise(sampleX, 0f) - 0.5f) * noiseAmplitude;

            points.Add(new Vector2(x, y));
        }

        // Обновляем визуал
        _lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            _lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0));
        }

        // Обновляем физику
        _edgeCollider.points = points.ToArray();
    }
}
    
    /*[SerializeField] private SpriteShapeController _spriteShapeController;
    
    [SerializeField, Range(3f,100f)] private int _levelLength;
    [SerializeField, Range(1f,50f)] private float _xMultiplier;
    [SerializeField, Range(1f,50f)] private float _yMultiplier;
    [SerializeField, Range(0f,1f)] private float _curveSmoothness;
    [SerializeField] private float _noiseStep;
    [SerializeField] private float _bottom;

    private Vector3 _lastPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnValidate(){
        _spriteShapeController.spline.Clear();

        for (int i = 0; i < _levelLength; i++){
            _lastPos = transform.position + new Vector3(i * _xMultiplier, Mathf.PerlinNoise(0,i*_noiseStep)*_yMultiplier);
            _spriteShapeController.spline.InsertPointAt(i,_lastPos);

            if (i !=0&&i!=_levelLength-1){
                _spriteShapeController.spline.SetTangentMode(i,ShapeTangentMode.Continuous);
                _spriteShapeController.spline.SetLeftTangent(i,Vector3.left*_xMultiplier*_curveSmoothness);
                _spriteShapeController.spline.SetRightTangent(i,Vector3.right*_xMultiplier*_curveSmoothness);
            }
        }

        _spriteShapeController.spline.InsertPointAt(_levelLength, new Vector3(_lastPos.x, transform.position.y - _bottom));
        _spriteShapeController.spline.InsertPointAt(_levelLength+1, new Vector3(transform.position.x, transform.position.y - _bottom));

        }*/


