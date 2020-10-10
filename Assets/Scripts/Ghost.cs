using UnityEngine;
using Random = UnityEngine.Random;

public class Ghost : MonoBehaviour
{
    [SerializeField] private Transform moveTransform;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    [SerializeField] private float onScreenTimeLeft;
    private float _timeLeft;
    private Vector2 _randomDirection;
    private int _currentLevel = 1;

    private const float MinX = -8.3f;
    private const float MaxX =  8.4f;
    private const float MinY = -4.1f;
    private const float MaxY =  4.0f;

    void Start()
    {
        _timeLeft = waitTime;
        _randomDirection = new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
        moveTransform.position = _randomDirection;
    }
    
    void Update()
    {
        AdjustSpeed();
        GhostLifespan();
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
    
    private void GhostLifespan()
    {
        if (onScreenTimeLeft < 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            onScreenTimeLeft -= Time.deltaTime;
        }
    }
    
    private void AdjustSpeed()
    {
        if (_currentLevel != Player.Level)
        {
            speed += Player.Level * 0.25f;
            _currentLevel++;
        }
    }
}
