using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject modelCopie;
    [SerializeField] private Image modelDisplayedTop;
    [SerializeField] private Image modelCopieDisplayedTop;
    [SerializeField] private Image modelDisplayedBody;
    [SerializeField] private Image modelCopieDisplayedBody;
    [SerializeField] private Image modelDisplayedWings;
    [SerializeField] private Image modelCopieDisplayedWings;
    [SerializeField] private AdsPopUp adsPopUp;
    [SerializeField] private Pause pause;
    [SerializeField] private Button buttonPause;

    bool isDraging = false;
    Vector2 startTouch;
    Vector2 swipeDelta;
    float droppingSpeed = 60;
    float shipRotation = 0;
    bool isResetRotation = false;
    float rotation = 0;
    float rotZ;
    Vector3 lastPos;
    Vector3 deltaPos;
    Vector3 startPos;
    float RotationMax = 0.8f; //60  ///25   ///3.33 ////66
    bool popUpOpen = false;
    bool gyroscopeEnabled = false;
    //GameObject modelCopie = null;
    bool isDeadSetup = false;

    void Start()
    {
        startPos = model.transform.position;
        int gyro = PlayerPrefs.GetInt("gyroscopeEnabled", 0);
        if (gyro == 0)
            gyroscopeEnabled = false;
        else if (gyro == 1)
            gyroscopeEnabled = true;
        rotZ = model.transform.rotation.z;
    }

    void Update()
    {
        //GyroMovements();
        //MovementInput();
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
        {
            if(!isDeadSetup)
			{
                modelCopie.SetActive(true);
                modelCopie.transform.position = model.transform.position;
                modelCopieDisplayedTop.sprite = modelDisplayedTop.sprite;
                modelCopieDisplayedBody.sprite = modelDisplayedBody.sprite;
                modelCopieDisplayedWings.sprite = modelDisplayedWings.sprite;
                model.transform.position = new Vector3(model.transform.position.x, model.transform.position.y, 50);
                isDeadSetup = true;
			}
            else
                DropShip();
        }

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

        /*swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
        }*/

        //Vector2 curDist = Input.GetTouch(0).position;
        //difference in previous locations using delta positions
        //Vector2 prevDist = (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) /*/ Input.GetTouch(0).deltaTime*/;

        //donne la difference de distance entre la frame precedente et l'actuel
        //float touchDelta = (curDist.x - prevDist.x) / Input.GetTouch(0).deltaTime;

        /*float rotationMax = 15f;
        if (touchDelta > rotationMax)
            touchDelta = rotationMax;
        else if (touchDelta < -rotationMax)
            touchDelta = -rotationMax;*/

        lastPos.x = model.transform.position.x;

        //if (Input.touches.Length > 0)
        //model.transform.position = new Vector3(model.transform.position.x + Input.touches[0].deltaPosition.x / 23, model.transform.position.y, model.transform.position.z); //25

        //if (Input.touches.Length > 0)
        //model.transform.position = new Vector3(model.transform.position.x + (Input.touches[0].deltaPosition.x / (Input.touches[0].deltaTime + 0.0000001f)) / 1000, model.transform.position.y, model.transform.position.z); //25

        if (Input.touches.Length > 0)
            model.transform.position += new Vector3(Input.touches[0].deltaPosition.x / (Input.touches[0].deltaTime + 0.0000001f) /19f, 0, 0) * Time.deltaTime;

        deltaPos.x = (model.transform.position.x - lastPos.x);

        if (Mathf.Abs(deltaPos.x) > RotationMax)
        {
            if (deltaPos.x > 0)
                deltaPos.x = RotationMax;
            else if (deltaPos.x < 0)
                deltaPos.x = -RotationMax;
        }

        float rotation = -(Input.touches[0].deltaPosition.x / (Input.touches[0].deltaTime + 0.0000001f)) / 200f;
        model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, rotation);

        //model.transform.rotation = new Quaternion(model.transform.rotation.x, model.transform.rotation.y, -deltaPos.x / 10f, model.transform.rotation.w); //2  2.5*/

        if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            model.transform.eulerAngles = new Vector3(0, 0, 0);

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
            rotation = 0;
        }
        else
		{
            if(model.transform.rotation.z > 0)
			{
                model.transform.rotation = new Quaternion(model.transform.rotation.x, model.transform.rotation.y, rotation, model.transform.rotation.w);
                if (model.transform.rotation.z > 180)
                    rotation += 100 * Time.deltaTime;
                else
                    rotation -= 100 * Time.deltaTime;
            }
            else if (model.transform.rotation.z < 0)
            {
                model.transform.rotation = new Quaternion(model.transform.rotation.x, model.transform.rotation.y, rotation, model.transform.rotation.w);
                rotation += 100 * Time.deltaTime;
            }
        }*/

        float rotMax = 4f;

        if (model.transform.eulerAngles.z > -rotMax && model.transform.eulerAngles.z < rotMax)
        {
            model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, 0);
            isResetRotation = false;
        }
        else
        {
            if (model.transform.eulerAngles.z > rotMax)
            {
                if (model.transform.rotation.z > 180)
                    model.transform.eulerAngles += new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, 300) * Time.deltaTime;
                else
                    model.transform.eulerAngles -= new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, 300) * Time.deltaTime;
            }
            else if (model.transform.rotation.z < rotMax)
            {
                model.transform.eulerAngles += new Vector3(model.transform.eulerAngles.x, model.transform.eulerAngles.y, 300) * Time.deltaTime;
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
        if (modelCopie.transform.position.y <= -200)
        {
            //set watch ads popup
            if (!popUpOpen)
            {
                //if (AdManager.instance.GetIsAdReviveLoaded() || AdManager.instance.GetIsAdMoneyLoaded())
                //{
                    adsPopUp.OpenPopUp();
                    popUpOpen = true;
                    buttonPause.interactable = false;
                //}
                //else
                    //GameManager.instance.SetGameState(GameManager.GameState.SCORE);
            }
        }
        else
		{
            if (CharacterManager.instance.GetHasShield())
                CharacterManager.instance.RemoveShield();

            modelCopie.transform.position -= new Vector3(0, GameManager.instance.GetScrolingSpeed() * droppingSpeed, 0) * Time.deltaTime;
            droppingSpeed += 100 * Time.deltaTime; //330
            modelCopie.transform.rotation = new Quaternion(modelCopie.transform.rotation.x, modelCopie.transform.rotation.y, modelCopie.transform.rotation.z + shipRotation, modelCopie.transform.rotation.w);
            shipRotation += 0.75f * Time.deltaTime; //0.01 //0.2
        }
    }

    void ReviveMovement()
    {
        droppingSpeed = 60;
        shipRotation = 0;
        modelCopie.transform.rotation = new Quaternion();
        if (modelCopie.transform.position.y < startPos.y)
        {
            modelCopie.transform.position += new Vector3(0, 80, 0) * Time.deltaTime;
            CharacterManager.instance.SetFuel(150);
        }
        else
        {
            model.transform.position = new Vector3(model.transform.position.x, model.transform.position.y, 0);
            modelCopie.SetActive(false);
            GameManager.instance.ResetGameEnd();
            popUpOpen = false;
            isDeadSetup = false;
            deltaPos.x = 0;
            lastPos.x = model.transform.position.x;
            GameManager.instance.SetGameState(GameManager.GameState.GAME);
            //SoundManager.instance.UnPauseMusic();
            GameManager.instance.SetReviveReward(false);
            buttonPause.interactable = true;
        }
    }
}
