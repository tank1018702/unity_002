using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBallCam : MonoBehaviour
{

    Transform ball;
    float baseY;
    float camOffsetY;

    public static bool startShake = false;  //camera是否开始震动
    public static float seconds = 0f;    //震动持续秒数
    public static bool started = false;    //是否已经开始震动
    public static float quake = 0.2f;       //震动系数

    private Vector3 camPOS;  //camera的起始位置

    void Start ()
    {
        ball = GameObject.FindGameObjectWithTag("Player").transform;
       
        camOffsetY = ball.transform.position.y - transform.position.y;
    }
	
	void Update ()
    {
        //当球的位置低于偏移值时让摄像机和球一起动
		float diffY = ball.transform.position.y - transform.position.y - camOffsetY;
        if (diffY < 0)
        {
            // 比必要的位置再低一些，可以防止抖动
            transform.position += new Vector3(0, diffY -0.15f, 0);
        }
        camPOS = transform.position;
    }

    void LateUpdate()
    {
        if (startShake)
        {
            Vector3 shakepos = Random.insideUnitSphere * quake;
            transform.position = camPOS + shakepos;
            //transform.localPosition = camPOS;
        }

        if (started)
        {
            StartCoroutine(WaitForSecond(seconds));
            started = false;
        }
    }
    /// <summary>
    /// 外部调用控制camera震动
    /// </summary>
    /// <param name="a">震动时间</param>
    /// <param name="b">震动幅度</param>
    public static void ShakeFor(float a, float b)
    {
        //		if (startShake)
        //			return;
        seconds = a;
        started = true;
        startShake = true;
        quake = b;
    }
    IEnumerator WaitForSecond(float a)
    {
        //		camPOS = transform.position;

        yield return new WaitForSeconds(a);
        startShake = false;
        transform.position=new Vector3(ball.transform.position.x,camPOS.y,-10);
    }

}
