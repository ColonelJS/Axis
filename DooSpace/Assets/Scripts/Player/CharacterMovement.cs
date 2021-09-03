using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private AdsPopUp adsPopUp;
    [SerializeField] private Pause pause;

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
    float RotationMax = 66f; //60  ///25   ///3.33
    bool popUpOpen = false;
    bool gyroscopeEnabled = false;
    void Start()
    {
        startPos = model.transform.position;
        int gyro = PlayerPrefs.GetInt("gyroscopeEnabled", 0);
        if (gyro == 0)
            gyroscopeEnabled = false;
        else if (gyro == 1)
            gyroscopeEnabled = true;
    }

    void Update()
    {
        //GyroMovements();
        if (GameManager.instance.GetGameState() == GameManager.GameState.GAME || GameManager.instance.GetGameState() == GameManager.GameState.ALIEN_WAVE)
        {
#if UNITY_ANDROID
            if (!pause.GetIsPause())
            {
                if (!gyroscopeEnabled)
                    MovementInput();
                else
                    GyroMovements();
            }
#endif
        }
        else if (GameManager.instance.GetGameState() == GameManager.GameState.LOSE)
            DropShip();

        if (Input.GetKey(KeyCode.Q))
            MoveCharacter(-150);
        else if (Input.GetKey(KeyCode.D))
            MoveCharacter(150);

        //if (isResetRotation)
            //ResetRotation();

        if(GameManager.instance.GetReviveReward())
		{
            ReviveMovement();
        }
    }

    /*float GetAngleByDeviceAxis(Vector3 axis)
    {
        //Quaternion deviceRotation = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(0, 0, 1, 0);
        Quaternion deviceRotation = new Quaternion(0, 0, 1, 0) * Input.gyro.attitude * new Quaternion(0, 1, 0, 0);
        Quaternion eliminationOfOthers = Quaternion.Inverse(
            Quaternion.FromToRotation(axis, deviceRotation * axis)
        );
        Vector3 filteredEuler = (eliminationOfOthers * deviceRotation).eulerAngles;

        float result = filteredEuler.z;
        if (axis == Vector3.up)
        {
            result = filteredEuler.y;
        }
        if (axis == Vector3.right)
        {
            // incorporate different euler representations.
            result = (filteredEuler.y > 90 && filteredEuler.y < 270) ? 180 - filteredEuler.x : filteredEuler.x;
        }
        return result;
    }*/

    float GetZRotation()
    {
        Quaternion deviceRotation = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * Input.gyro.attitude * new Quaternion(1, 0, 1, 0);
        Quaternion eliminationOfXY = Quaternion.Inverse(
            Quaternion.FromToRotation(Quaternion.identity * Vector3.right,
                                  deviceRotation * Vector3.up)
            );
        Quaternion rotationZ = eliminationOfXY * deviceRotation;
        float roll = rotationZ.eulerAngles.y;

        return roll;
    }

    void GyroMovements()
	{
        Input.gyro.enabled = true;
        float rotZ = Input.gyro.rotationRateUnbiased.y * 10;

        if (rotZ > 45)
            rotZ = 45;
        else if (rotZ < -45)
            rotZ = -45;

        float rotFactor = 0.92f;
        model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, -rotZ * rotFactor);

        //float rotY = Input.gyro.attitude.eulerAngles.y;
        float rotY = GetZRotation();

        if (rotY > 180)
		{
            rotY = 360 - GetZRotation();
            if (rotY > 45)
                rotY = 45;
        }
        else if(rotY < 180)
		{
            rotY = -rotY;
            if (rotY < -45)
                rotY = -45;
        }

        float factor = 1.1f;
        model.transform.position = new Vector3((rotY * factor), model.transform.position.y, model.transform.position.z);
        if (model.transform.localPosition.x > 450)
            model.transform.localPosition = new Vector3(450, model.transform.localPosition.y, model.transform.localPosition.z);
        else if (model.transform.localPosition.x < -450)
            model.transform.localPosition = new Vector3(-450, model.transform.localPosition.y, model.transform.localPosition.z);
    }

    public void SetGyroscope(bool _value)
	{
        gyroscopeEnabled = _value;

        int gyro = 0;
        if (_value == true)
            gyro = 1;

        PlayerPrefs.SetInt("gyroscopeEnabled", gyro);
    }

    void MovementInput()
	{
        /*if (Input.touches.Length > 0)
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

        }*/

       /* swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
        }*/

        lastPos.x = model.transform.position.x;

        if (Input.touches.Length > 0)
            model.transform.position = new Vector3(model.transform.position.x + Input.touches[0].deltaPosition.x/23, model.transform.position.y, model.transform.position.z); //25

        //model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, Input.touches[0].deltaPosition.x / 23) * Time.deltaTime;
        deltaPos.x = (model.transform.position.x - lastPos.x)*10;

        if(deltaPos.x > RotationMax)
            deltaPos.x = RotationMax;
        else if(deltaPos.x < -RotationMax)
            deltaPos.x = -RotationMax;

        //model.transform.rotation = Quaternion.AngleAxis(-deltaPos.x * 50, Vector3.forward);
        model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, -deltaPos.x * 66) * Time.deltaTime;
        //new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.x, -deltaPos.x * 1000)
        //model.transform.rotation = Quaternion.Lerp(model.transform.rotation, Quaternion.Euler(model.transform.eulerAngles.x, model.transform.eulerAngles.y, deltaPos.x * 45f), 3f*Time.deltaTime);
        //model.transform.eulerAngles += new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, deltaPos.x*500) * Time.deltaTime;

        /*if (model.transform.eulerAngles.z > 2)
            model.transform.eulerAngles += new Vector3(0, 0, 33) * Time.deltaTime;
        else if (model.transform.eulerAngles.z < 2)
            model.transform.eulerAngles -= new Vector3(0, 0, 33) * Time.deltaTime;
        else
            model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, 0) * Time.deltaTime;*/

        if (model.transform.localPosition.x > 500)
            model.transform.localPosition = new Vector3(500, model.transform.localPosition.y, model.transform.localPosition.z);
        else if (model.transform.localPosition.x < -500)
            model.transform.localPosition = new Vector3(-500, model.transform.localPosition.y, model.transform.localPosition.z);
    }

    void ResetRotation()
	{
        /*if (model.transform.rotation.z > -0.1 && model.transform.rotation.z < 0.1)
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
        }*/

        float rotMax = 2.5f;

        if (model.transform.eulerAngles.z > -rotMax && model.transform.eulerAngles.z < rotMax)
        {
            model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, 0);
            isResetRotation = false;
        }
        else
        {
            if (model.transform.eulerAngles.z > rotMax)
            {
                model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, rotation);
                rotation -= 750 * Time.deltaTime;

                //model.transform.eulerAngles += new Vector3(0, 0, model.transform.eulerAngles.z);
            }
            else if (model.transform.rotation.z < rotMax)
            {
                model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, rotation);
                rotation += 750 * Time.deltaTime;
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
            //SoundManager.instance.UnPauseMusic();
            GameManager.instance.SetReviveReward(false);
        }
    }
}
