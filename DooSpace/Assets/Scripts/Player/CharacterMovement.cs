using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private AdsPopUp adsPopUp;

    bool isDraging = false;
    Vector2 startTouch;
    Vector2 swipeDelta;
    float droppingSpeed = 60;
    float shipRotation = 0;
    bool isResetRotation = false;
    float rotationSpeed = 10f;
    float rotation = 0;
    Vector3 savedPos;
    Vector3 lastPos;
    Vector3 deltaPos;
    Vector3 startPos;
    int RotationMax = 25; //60
    bool popUpOpen = false;
    bool gyroscopeEnabled = true;
    void Start()
    {
        startPos = model.transform.position;
    }

    void Update()
    {
        GyroMovements();
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
        {
#if UNITY_ANDROID
            if (!gyroscopeEnabled)
                MovementInput();
            else
                GyroMovements();
#endif
        }
        else if (GameManager.instance.GetGameState() == GameManager.GameState.LOSE)
            DropShip();

        if (Input.GetKey(KeyCode.Q))
            MoveCharacter(-150);
        else if (Input.GetKey(KeyCode.D))
            MoveCharacter(150);

        if (isResetRotation)
            ResetRotation();

        if(GameManager.instance.GetReviveReward())
		{
            ReviveMovement();
        }
    }

    void GyroMovements()
	{
        Input.gyro.enabled = true;
        //Debug.Log("rotY : " + Input.gyro.attitude.eulerAngles.y);

        //model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, model.transform.eulerAngles.z + Input.gyro.rotationRateUnbiased.z * 2 * Time.deltaTime * Mathf.Rad2Deg);
        //model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, model.transform.eulerAngles.z 
        //    + Input.gyro.rotationRateUnbiased.z * 2 * Time.deltaTime * Mathf.Rad2Deg);

        /*float rotFactor = 10000;
        float newRotZ;
        newRotZ = 0 + Input.gyro.userAcceleration.y * rotFactor * Time.deltaTime;

        newRotZ = Mathf.Log(newRotZ);
        newRotZ = newRotZ * 10;

        if (newRotZ > 45)
            newRotZ = 45;
        else if(newRotZ < -45)
            newRotZ = -45;

        if (newRotZ > 0 && newRotZ < 0.8f)
            newRotZ = 0;
        else if(newRotZ < 0 && newRotZ > -0.8f)
            newRotZ = 0;

        model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, newRotZ);*/

        float rotZ = Input.gyro.rotationRateUnbiased.y * 10;

        if (rotZ > 45)
            rotZ = 45;
        else if (rotZ < -45)
            rotZ = -45;

        model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, -rotZ);

        Debug.Log("rot rate z : " + Input.gyro.rotationRateUnbiased.y);
        Debug.Log("rot z : " + rotZ);

        float rotY = Input.gyro.attitude.eulerAngles.y;
        if (rotY > 180)
		{
            rotY = 360 - Input.gyro.attitude.eulerAngles.y;
            if (rotY > 45)
                rotY = 45;
        }
        else if(rotY < 180)
		{
            rotY = -rotY;
            if (rotY < -45)
                rotY = -45;
        }

        float factor = 1.2f;
        model.transform.position = new Vector3((rotY * factor) + 20, model.transform.position.y, model.transform.position.z);

        if (model.transform.localPosition.x > 450)
            model.transform.localPosition = new Vector3(450, model.transform.localPosition.y, model.transform.localPosition.z);
        else if (model.transform.localPosition.x < -450)
            model.transform.localPosition = new Vector3(-450, model.transform.localPosition.y, model.transform.localPosition.z);

        /*float maxAngle = 10f;
        if (model.transform.eulerAngles.z > maxAngle && model.transform.eulerAngles.z < maxAngle)
            model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, 0);
        else
        {
            if (model.transform.eulerAngles.z > maxAngle)
                model.transform.eulerAngles -= new Vector3(0, 0, 300) * Time.deltaTime;
            else if(model.transform.eulerAngles.z < maxAngle)
                model.transform.eulerAngles += new Vector3(0, 0, 300) * Time.deltaTime;
        }*/
    }

    public void SetGyroscope(bool _value)
	{
        gyroscopeEnabled = _value;
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
                isResetRotation = true;
                rotation = model.transform.rotation.z;
            }

        }

        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
        }

        lastPos.x = model.transform.position.x;

        if (Input.touches.Length > 0)
            model.transform.position = new Vector3(model.transform.position.x + Input.touches[0].deltaPosition.x/24, model.transform.position.y, model.transform.position.z); //25

        deltaPos.x = model.transform.position.x - lastPos.x;

        if(Mathf.Abs(deltaPos.x) > RotationMax)
		{
            if (deltaPos.x > 0)
                deltaPos.x = RotationMax;
            else if (deltaPos.x < 0)
                deltaPos.x = -RotationMax;
        }

        model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.x, -deltaPos.x*900) * Time.deltaTime;

        if (model.transform.localPosition.x > 500)
            model.transform.localPosition = new Vector3(500, model.transform.localPosition.y, model.transform.localPosition.z);
        else if (model.transform.localPosition.x < -500)
            model.transform.localPosition = new Vector3(-500, model.transform.localPosition.y, model.transform.localPosition.z);
    }

    void ResetRotation()
	{
        if (model.transform.rotation.z > -0.1 && model.transform.rotation.z < 0.1)
		{
            model.transform.rotation = new Quaternion(model.transform.rotation.x, model.transform.rotation.y, 0, model.transform.rotation.w);
            isResetRotation = false;
        }
        else
		{
            if(model.transform.rotation.z > 0)
			{
                model.transform.rotation = new Quaternion(model.transform.rotation.x, model.transform.rotation.y, rotation, model.transform.rotation.w);
                rotation -= 100 * Time.deltaTime;
            }
            else if (model.transform.rotation.z < 0)
            {
                model.transform.rotation = new Quaternion(model.transform.rotation.x, model.transform.rotation.y, rotation, model.transform.rotation.w);
                rotation += 100 * Time.deltaTime;
            }
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
        if (model.transform.position.y <= -200)
        {
            //set watch ads popup
            if (!popUpOpen)
            {
                adsPopUp.OpenPopUp();
                popUpOpen = true;
            }
        }
        else
		{
            model.transform.position -= new Vector3(0, GameManager.instance.GetScrolingSpeed() * droppingSpeed, 0) * Time.deltaTime;
            droppingSpeed += 100 * Time.deltaTime; //330
            model.transform.rotation = new Quaternion(model.transform.rotation.x, model.transform.rotation.y, model.transform.rotation.z + shipRotation, model.transform.rotation.w);
            shipRotation += 0.75f * Time.deltaTime; //0.01 //0.2
        }
    }

    void ReviveMovement()
    {
        droppingSpeed = 60;
        shipRotation = 0;
        model.transform.rotation = new Quaternion();
        if (model.transform.position.y < startPos.y)
        {
            model.transform.position += new Vector3(0, 80, 0) * Time.deltaTime;
            CharacterManager.instance.SetFuel(150);
        }
        else
        {    
            model.transform.position = new Vector3(model.transform.position.x, startPos.y, model.transform.position.z);
            GameManager.instance.ResetGameEnd();
            popUpOpen = false;
            GameManager.instance.SetGameState(GameManager.GameState.GAME);
            GameManager.instance.SetReviveReward(false);
        }
    }
}
