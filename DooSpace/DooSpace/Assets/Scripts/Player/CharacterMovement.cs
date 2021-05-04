using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject model;

    bool isDraging = false;
    Vector2 startTouch;
    Vector2 swipeDelta;
    float droppingSpeed = 60;
    float shipRotation = 0;

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME)
            MovementInput();
        else if (GameManager.instance.GetGameState() == GameManager.GameState.LOSE)
            DropShip();

        if (Input.GetKey(KeyCode.Q))
            MoveCharacter(-150);
        else if (Input.GetKey(KeyCode.D))
            MoveCharacter(150);
    }

    void MovementInput()
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
            }

        }

        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
        }

        //Debug.Log("swipe delta : " + swipeDelta);

        float x = swipeDelta.x;
        float y = swipeDelta.y;
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            MoveCharacter(x);
        }
    }

    void MoveCharacter(float _amount)
	{
        float moveSpeed = _amount;
        model.transform.position += new Vector3(moveSpeed, 0, 0) * Time.deltaTime;
	}

    public float GetMoveDeltaX()
    {
        return swipeDelta.x;
    }

    void DropShip()
	{
        model.transform.position -= new Vector3(0, GameManager.instance.GetScrolingSpeed() * droppingSpeed, 0) * Time.deltaTime;
        //model.transform.position += new Vector3(0, droppingSpeed, 0) * Time.deltaTime;
        droppingSpeed += 330 * Time.deltaTime;
        //gameObject.transform.localRotation = new Quaternion(0, 0, (droppingSpeed / 1000) - 60, 0);
        //model.transform.rotation = new Quaternion(0, 0, shipRotation, 0);
        model.transform.rotation = new Quaternion(model.transform.rotation.x, model.transform.rotation.y, model.transform.rotation.z + shipRotation, model.transform.rotation.w);
        shipRotation += 0.01f * Time.deltaTime; //0.01

        if (model.transform.position.y <= -200)
        {
            GameManager.instance.SetGameState(GameManager.GameState.SCORE);
            print("game state set to score");
        }
    }
}
