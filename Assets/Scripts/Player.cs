﻿using TMPro;
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
    public static int Level;

    private void Update()
    {
        ShotBullet();
        if (_lives == 0)
        {
            GameOver();
        }
        InstantiateEnemies();
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
            var firstHit = true;
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            for (int i = 0; i < 2; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
                if (hit.collider != null && hit.transform.gameObject.CompareTag("Ghost"))
                {
                    firstHit = false;
                    _currentScore += 3;
                    _enemiesKilled++;
                    enemiesKilledWithOneBullet++;
                    Destroy(hit.transform.gameObject);
                }
                else if (firstHit)
                {
                    firstHit = false;
                    Destroy(hearts[_lives - 1]);
                    _lives -= 1;
                    _currentScore -= 1;
                }
            }

            if (enemiesKilledWithOneBullet == 2)
            {
                _currentScore += 5;
            }
            
            if (_enemiesKilled % 10 == 0 && _enemiesKilled != 0)
            {
                Level++;
            }

            levelText.text = "Level: " + (Level + 1);
            scoreText.text = "Score: " + _currentScore;
        }
    }

    private void InstantiateEnemies()
    {
        if (enemies.transform.childCount == 0)
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

    private static void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
