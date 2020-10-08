using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject[] hearts;

    private int _enemiesKilled = 0;
    private int _currentScore = 0;
    private int _lives = 5;
    public static int Level = 0;

    void Update()
    {
        ShotBullet();
        if (_lives == 0)
        {
            GameOver();
        }
    }

    void FixedUpdate()
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
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            
            if (hit.collider != null && hit.transform.gameObject.CompareTag("Ghost"))
            {
                _currentScore += 3;
                _enemiesKilled++;
                Destroy(hit.transform.gameObject);
            }
            else
            {
                Destroy(hearts[_lives - 1]);
                _lives -= 1;
                _currentScore -= 1;
            }

            if (_enemiesKilled % 10 == 0 && _enemiesKilled != 0)
            {
                Level++;
            }

            levelText.text = "Level: " + (Level + 1);
            scoreText.text = "Score: " + _currentScore;
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
