using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject enemies;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private double specialVersionTime;

    private int _enemiesKilled;
    private int _currentScore;
    private int _lives = 5;
    private int _witchAppearances;
    private int _witchHits;
    private double _timer;
    private bool _isSpecial;
    private const float DelayBetweenInputs = 0.1f;
    private float _lastShot;

    public static int Level;

    private void Start()
    {
        _timer = specialVersionTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            _isSpecial = true;
        }
        if (_isSpecial && _timer > 0)
        {
            ShotBulletSpecial();
            InstantiateGhosts(false);
            InstantiateWitch(100);
            _timer -= Time.deltaTime;
        }
        else
        {
            ShotBullet();
            InstantiateGhosts(true);
            InstantiateWitch(500);
        }
        if (_lives == 0)
        {
            GameOver();
        }

        if (_isSpecial && _timer < 0)
        {
            _timer -= Time.deltaTime;
        }
        
        if (_timer < -5) // must be at least 5 seconds between each time the player starts the variant mode
        {
            _timer = specialVersionTime;
            _isSpecial = false;
        }
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        if (cam is null) return;
        
        var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        var targetPos = new Vector3(mousePos.x, mousePos.y, 0);

        targetPos = ClampedPlayerPosition(targetPos);
        
        GetComponent<Rigidbody2D>().MovePosition(targetPos);
    }

    Vector3 ClampedPlayerPosition(Vector3 targetPos)
    {
        var screenLimits = new Vector3(Screen.width, Screen.height, 0);
        var outerLimit = cam.ScreenToWorldPoint(screenLimits);
        var maxHeight = outerLimit.y - GetComponentInChildren<Renderer>().bounds.extents.y;
        var maxWidth = outerLimit.x - GetComponentInChildren<Renderer>().bounds.extents.x;

        var lockedPosY = Mathf.Clamp(targetPos.y, -maxHeight, maxHeight);
        var lockedPosX = Mathf.Clamp(targetPos.x, -maxWidth, maxWidth);
        
        return new Vector3(lockedPosX, lockedPosY, 0);
    }

    private void ShotBulletSpecial()
    {
        if (Input.GetMouseButton(0) && Time.time > _lastShot + DelayBetweenInputs)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D[] enemiesHit = Physics2D.RaycastAll(mousePos2D, Vector2.zero );
            
            if (enemiesHit == null || enemiesHit.Length == 0)
            {
                _currentScore -= 2; 
            }
            else
            {
                PointsWonForTheShot(enemiesHit, false);
            }
            ChangeLevelCheck();
            _lastShot = Time.time;
        }
    }

    private void ShotBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D[] enemiesHit = Physics2D.RaycastAll(mousePos2D, Vector2.zero );
            
            if (enemiesHit == null || enemiesHit.Length == 0)
            {
                Destroy(hearts[_lives - 1]);
                _lives -= 1;
                _currentScore -= 1; 
            }
            else
            {
                PointsWonForTheShot(enemiesHit, true);
            }
            
            ChangeLevelCheck();
        }
    }

    private void PointsWonForTheShot(RaycastHit2D[] enemiesHit, bool isRegular)
    {
        var enemiesKilledWithOneBullet = 0;
        foreach (var enemy in enemiesHit)
        {
            if (enemy.transform.gameObject.CompareTag("Ghost"))
            {
                _currentScore = isRegular ? (_currentScore + 3) : (_currentScore + 1);
                _enemiesKilled++;
                enemiesKilledWithOneBullet++;
                Destroy(enemy.transform.gameObject);
            }
            else if (enemy.transform.gameObject.CompareTag("Witch"))
            {
                _witchHits++;
                if (_witchHits == 3)
                {
                    _witchHits = 0;
                    _currentScore = isRegular ? (_currentScore + 5) : (_currentScore + 1);
                    Destroy(enemy.transform.gameObject);
                }
            }
        }
        if (enemiesKilledWithOneBullet >= 2)
        {
            _currentScore += 5;
        }
    }

    private void ChangeLevelCheck()
    {
        if (_enemiesKilled >= 10 && _enemiesKilled != 0 && _witchAppearances >= 2)
        {
            _enemiesKilled = 0;
            _witchAppearances = 0;
            Level++;
        }

        levelText.text = "Level: " + (Level + 1);
        scoreText.text = "Score: " + _currentScore;
    }
    
    private void InstantiateGhosts()
    {
        var enemyColor = Random.Range(0, 3);
        switch (enemyColor)
        {
            case 0:
                Instantiate(enemyPrefabs[0], enemies.transform);
                break;
            case 1:
                Instantiate(enemyPrefabs[1], enemies.transform);
                break;
            case 2:
                Instantiate(enemyPrefabs[2], enemies.transform);
                break;
        }
    }

    private void InstantiateGhosts(bool isRegular)
    {
        if (isRegular && !FindGameObjectInChildWithTag(enemies, "Ghost"))
        {
            for (var i = 0; i < 2; i++)
            {
                InstantiateGhosts();
            }
        }
        else if (!isRegular)
        {
            if (0 == Random.Range(0, 10))
            {
                InstantiateGhosts();
            }
        }
    }

    private void InstantiateWitch(int odds)
    {
        if (0 == Random.Range(0, odds))
        {
            _witchAppearances++;
            Instantiate(enemyPrefabs[3], enemies.transform);
        }
    }

    private static void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private bool FindGameObjectInChildWithTag(GameObject parent, string matchedTag)
    {
        for (var i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.CompareTag(matchedTag))
            {
                return true;
            }
        }
        return false;
    }
}
