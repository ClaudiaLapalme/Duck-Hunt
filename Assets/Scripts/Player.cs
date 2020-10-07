using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private TextMeshProUGUI scoreText;
    private int _currentScore = 0;

    void Update()
    {
        ShotBullet();   
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
                Destroy(hit.transform.gameObject);
            }
            else
            {
                _currentScore -= 1;
            }
            scoreText.text = "Score: " + _currentScore;
        }
    }
}
