using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Transform moveTransform;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    private float _timeLeft;
    private Vector2 _randomDirection;

    private const float MinX = -12.5f;
    private const float MaxX =  7.1f;
    private const float MinY = -5.7f;
    private const float MaxY =  2.8f;

    void Start()
    {
        _timeLeft = waitTime;
        _randomDirection = new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
        moveTransform.position = _randomDirection;
    }
    
    void Update()
    {
        CalculateMovementVector();
    }

    private void CalculateMovementVector()
    {
        transform.position = Vector2.MoveTowards(transform.position, _randomDirection, speed * Time.deltaTime);
        
        if (_timeLeft < 0)
        {
            _randomDirection = new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
            _timeLeft = waitTime;
        }
        else
        {
            _timeLeft -= Time.deltaTime;
        }
    }
}
