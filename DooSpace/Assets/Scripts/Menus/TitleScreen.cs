using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject highscoreMenu;
    [SerializeField] private GameObject customMenu;

    bool isDraging = false;
    Vector2 startTouch;
    Vector2 swipeDelta;
    bool swipeLeft = false;
    bool swipeRight = false;

    bool isHighscoreOpen = false;
    bool isCustomOpen = false;

    float moveBackSpeed = 100f;

    void Start()
    {
        
    }

    void Update()
    {
        UpdateMenuSwipe();
    }

    void UpdateMenuSwipe()
	{
        if (GetSwipe())
        {
            if (!isCustomOpen)
                if (swipeLeft)
                    MoveMenuRelativeToSwipe("left");
            else
                if (swipeRight)
                    MoveMenuBackRelativeToSwipe("right");

            if (!isHighscoreOpen)
                if (swipeRight)
                    MoveMenuRelativeToSwipe("right");
            else                           
                if (swipeLeft)
                    MoveMenuBackRelativeToSwipe("left");
        }
        else
		{
            if (!isCustomOpen)
                MoveBackMenu("right");
            else
                MoveBackMenuReverse("left");
        }

    }

    void MoveMenuRelativeToSwipe(string _direction)
	{
        if(_direction == "left")
		{
            if (customMenu.transform.localPosition.x > 0 && Input.touches.Length > 0)
                customMenu.transform.localPosition -= new Vector3((Input.touches[0].deltaPosition.x / 50), 0, 0) * Time.deltaTime;//25
            else
            {
                customMenu.transform.localPosition = new Vector3(0, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = true;
            }
        }
        if (_direction == "right")
        {
            if (highscoreMenu.transform.localPosition.x < 0 && Input.touches.Length > 0)
                highscoreMenu.transform.localPosition += new Vector3((Input.touches[0].deltaPosition.x / 50), 0, 0) * Time.deltaTime;
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(0, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = true;
            }
        }
    }

    void MoveMenuBackRelativeToSwipe(string _direction)
    {
        if (_direction == "right")
        {
            if (customMenu.transform.localPosition.x < Screen.width && Input.touches.Length > 0)
                customMenu.transform.localPosition += new Vector3((Input.touches[0].deltaPosition.x / 50), 0, 0) * Time.deltaTime;
            else
            {
                customMenu.transform.localPosition = new Vector3(Screen.width, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
                isCustomOpen = false;
            }
        }
        if (_direction == "left")
        {
            if (highscoreMenu.transform.localPosition.x > -Screen.width && Input.touches.Length > 0)
                highscoreMenu.transform.localPosition -= new Vector3((Input.touches[0].deltaPosition.x / 50), 0, 0) * Time.deltaTime;
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(-Screen.width, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
                isHighscoreOpen = false;
            }
        }
    }

    void MoveBackMenu(string _direction)
	{
        if (_direction == "left")
        {
            if (highscoreMenu.transform.localPosition.x > -Screen.width)
                highscoreMenu.transform.localPosition -= new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(-Screen.width, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
            }
        }

        if (_direction == "right")
        {
            if (customMenu.transform.localPosition.x < Screen.width)
                customMenu.transform.localPosition += new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            else
            {
                customMenu.transform.localPosition = new Vector3(Screen.width, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
            }
        }
    }

    void MoveBackMenuReverse(string _direction)
    {
        if (_direction == "right")
        {
            if (highscoreMenu.transform.localPosition.x < 0)
                highscoreMenu.transform.localPosition += new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            else
            {
                highscoreMenu.transform.localPosition = new Vector3(0, highscoreMenu.transform.localPosition.y, highscoreMenu.transform.localPosition.z);
            }
        }

        if (_direction == "left")
        {
            if (customMenu.transform.localPosition.x > 0)
                customMenu.transform.localPosition -= new Vector3(moveBackSpeed, 0, 0) * Time.deltaTime;
            else
            {
                customMenu.transform.localPosition = new Vector3(0, customMenu.transform.localPosition.y, customMenu.transform.localPosition.z);
            }
        }
    }

    bool GetSwipe()
	{
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                swipeLeft = false;
                swipeRight = false;
                return false;
            }

        }

        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
        }

        float x = swipeDelta.x;
        float y = swipeDelta.y;
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            //Left or right
            if (x < 0)
                swipeLeft = true;
            else
                swipeRight = true;
            return true;
        }
        return false;
    }
}
