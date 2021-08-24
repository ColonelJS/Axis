using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    [SerializeField] RectTransform panoClouds1;
    [SerializeField] RectTransform panoClouds2;
    [SerializeField] float cloudsSpeed = 50;
    [Space(10)]
    [SerializeField] RectTransform panoCountry1;
    [SerializeField] RectTransform panoCountry2;
    [SerializeField] float countrySpeed = 30;
    [SerializeField] float earthRotationSpeed = 1200;
    [SerializeField] float cooldownEarthRotate = 20f;

    Vector3 startPanoPos;
    Vector3 endPanoPos;
    Vector3 startCountryPos;

    float baseCooldownEarthRotate;
    bool earthRotate = false;
    bool countryReset = false;

    List<RectTransform> listClouds = new List<RectTransform>();
    List<RectTransform> listCountry = new List<RectTransform>();
    void Start()
    {
        ////Clouds
        panoClouds2.transform.localPosition = panoClouds1.transform.localPosition + new Vector3(0, panoClouds1.rect.height, 0);
        startPanoPos = panoClouds2.transform.localPosition;
        endPanoPos = panoClouds1.transform.localPosition - new Vector3(0, panoClouds1.rect.height, 0);
        listClouds.Add(panoClouds1);
        listClouds.Add(panoClouds2);

        panoCountry2.transform.localPosition = startPanoPos;
        listCountry.Add(panoCountry1);
        listCountry.Add(panoCountry2);

        baseCooldownEarthRotate = cooldownEarthRotate;
    }

    void Update()
    {
        UpdateCloudsMovements();
        UpdateCountryMovements();
        UpdateEarthRotation();
    }

    void UpdateCloudsMovements()
	{
        for (int i = 0; i < listClouds.Count; i++)
        {
            listClouds[i].transform.localPosition -= new Vector3(0, cloudsSpeed, 0) * Time.deltaTime;
            if (listClouds[i].transform.localPosition.y <= endPanoPos.y)
            {
                listClouds[i].transform.localPosition = startPanoPos;
            }
        }
    }

    void UpdateCountryMovements()
    {
        for (int i = 0; i < listCountry.Count; i++)
        {
            listCountry[i].transform.localPosition -= new Vector3(0, countrySpeed, 0) * Time.deltaTime;
            if (listCountry[i].transform.localPosition.y <= endPanoPos.y)
            {
                listCountry[i].transform.localPosition = startPanoPos;
            }
        }
    }

    void UpdateEarthRotation()
	{
        if (cooldownEarthRotate <= 0)
        {
            earthRotate = true;
            Debug.Log("earth rotation start");
            startCountryPos = listCountry[0].transform.localPosition;
            cooldownEarthRotate = baseCooldownEarthRotate;
        }
        else
        {
            if(!earthRotate)
                cooldownEarthRotate -= Time.deltaTime;
        }

        if(earthRotate)
		{
            for (int i = 0; i < listCountry.Count; i++)
            {
                listCountry[i].transform.localPosition -= new Vector3(0, earthRotationSpeed, 0) * Time.deltaTime;
                //listClouds[i].transform.localPosition -= new Vector3(0, earthRotationSpeed, 0) * Time.deltaTime;
                if (listCountry[i].transform.localPosition.y <= endPanoPos.y)
                {
                    if (i == 0)
                        countryReset = true;
                    listCountry[i].transform.localPosition = startPanoPos;
                }
            }

            if (listCountry[0].transform.localPosition.y <= startCountryPos.y/10 && countryReset)
            {
                earthRotate = false;
                countryReset = false;
                Debug.Log("earth rotation end");
            }
        }
    }
}
