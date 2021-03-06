﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBox : MonoBehaviour
{

    //public Color colorStart = Color.blue;
    //public Color colorEnd = Color.green;

    public Material SkyBoxNight;
    public Material SkyBoxMorning;
    public float duration = 1.0F;


    [Range(0 , 10)]
    public float nightmorningtime = 0f;

    

    bool testbool = false;

    private Color CurrentMorningColor;
    private Color CurrentNightColor;

    public GameObject Moon;
    public GameObject Sun;

    Vector3 moonpos;
    Vector3 Sunpos;

    private bool isMoon = true;
    private bool isOneSelect = false;
    private float moonspeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        RenderSettings.skybox = SkyBoxNight;
        RenderSettings.skybox.SetColor("_Tint", new Color(1, 1, 1));
        RenderSettings.skybox = SkyBoxMorning;
        RenderSettings.skybox.SetColor("_Tint", new Color(0.3f, 0.3f, 0.3f));
        RenderSettings.skybox = SkyBoxNight;



        if (Moon == null) return;
        moonpos = Moon.transform.position;
        Sunpos = Sun.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //float lerp = Mathf.PingPong(Time.time, duration) / duration;
        //RenderSettings.skybox.SetColor("_Tint", Color.Lerp(colorStart, colorEnd, lerp));

        if (Moon == null) return;


        SunAndMoonMoove();


        if (!isOneSelect)
        {
            StartCoroutine(StartMorning());
        }
    }

    IEnumerator StartMorning()
    {
        Color tintcolor = RenderSettings.skybox.GetColor("_Tint");
       
        while (tintcolor.r > 0f && !testbool)
        {
            tintcolor -= new Color(0.1f, 0.1f, 0.1f, 1) * Time.deltaTime * nightmorningtime / 2;
            RenderSettings.skybox.SetColor("_Tint", tintcolor);

            yield return new WaitForSeconds(0f);
        }



        testbool = true;



       

        while (tintcolor.r < 1.0f && testbool)
        {

            isMoon = false;
            RenderSettings.skybox = SkyBoxMorning;

            tintcolor += new Color(0.1f, 0.1f, 0.1f, 1) * Time.deltaTime * nightmorningtime;

            RenderSettings.skybox.SetColor("_Tint", tintcolor);

          

            yield return new WaitForSeconds(0f);
        }

        isOneSelect = true;
    }






    private void OnApplicationQuit()
    {
    }



    void SunAndMoonMoove()
    {

        if(Moon.transform.position.y >= -200f)
        {
            Sun.SetActive(false);
            moonpos.y -= moonspeed * Time.deltaTime;

            Moon.transform.position = moonpos;
        }
        else if(Sun.transform.position.y <= 39f)
        {
            Sun.gameObject.SetActive(true);
            Moon.gameObject.SetActive(false);
            Sunpos.y += moonspeed * Time.deltaTime;

            Sun.transform.position = Sunpos;
        }
        


    }

}
