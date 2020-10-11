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

    private int _enemiesKilled;
    private int _currentScore;
    private int _lives = 5;
    private int _witchAppearances;
    private int _witchHits;

    public static int Level;

    private void Update()
    {
        ShotBullet();
        if (_lives == 0)
        {
            GameOver();
        }
        InstantiateGhosts();
        InstantiateWitch();
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

    void ShotBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var enemiesKilledWithOneBullet = 0;
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
                foreach (var enemy in enemiesHit)
                {
                    if (enemy.transform.gameObject.CompareTag("Ghost"))
                    {
                        _currentScore += 3;
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
                            _currentScore += 5;
                            Destroy(enemy.transform.gameObject);
                        }
                    }
                }
            }

            if (enemiesKilledWithOneBullet == 2)
            {
                _currentScore += 5;
            }

            if (_enemiesKilled % 10 == 0 && _enemiesKilled != 0 && _witchAppearances >= 2)
            {
                _witchAppearances = 0;
                Level++;
            }

            levelText.text = "Level: " + (Level + 1);
            scoreText.text = "Score: " + _currentScore;
        }
    }

    private void InstantiateGhosts()
    {
        if (!FindGameObjectInChildWithTag(enemies, "Ghost"))
        {
            for (var i = 0; i < 2; i++)
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
        }
    }

    private void InstantiateWitch()
    {
        if (0 == Random.Range(0, 500))
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
