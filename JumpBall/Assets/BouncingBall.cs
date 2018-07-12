using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBall : MonoBehaviour
{

    SphereCollider sphereCollider;

    public float bounceSpeed;
    public float gravity;
    public float maxSpeed = 1;

    float radius = 0.5f;
    float speed;
    float origScale;
    public float scale;

    public GameObject prefabDecal;
    public GameObject pieRoot;

    GameMode gamemode;

    public float lastBounceTime;

    void Start ()
    {
        gamemode = GameObject.Find("GameMode").GetComponent<GameMode>();
        sphereCollider = GetComponent<SphereCollider>();
        radius = sphereCollider.radius;
        origScale = transform.localScale.x;
        scale = origScale;
	}

    void AddDecal()
    {
        if (prefabDecal == null || pieRoot == null)
        {
            return;
        }
        // 当前球的底部和平面略有差异，需要求准确的平面位置：
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100))
        {
            // 生成贴花
            var rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(prefabDecal, hit.point+new Vector3(0,0.3f,0), rot, pieRoot.transform);
        }
    }

    void Bounce()
    {
        //当球反弹回升的时候跳过对圆片的检测
        if (speed >= 0)
        {
            return;
        }

        // 确定一个位置合适的立方体，比球略小，位置偏下
        Vector3 p = transform.position + new Vector3(0, -radius, 0);
        Vector3 size = new Vector3(radius * 0.5f, radius * 0.5f, radius * 0.5f);

        Collider[] col = Physics.OverlapBox(p, size, Quaternion.identity, LayerMask.GetMask("Default"));
        if (col.Length > 0)
        {
            foreach(var c in col)
            {
                if (c.transform.tag == "Obstacle")
                {
                    gamemode.GameOver();
                }
            }

            lastBounceTime = Time.time;
            speed = bounceSpeed;
            //模拟球的反弹形变效果
            scale = origScale * 0.3f;
            AddDecal();
        }
    }

    void Drop()
    {
        //每帧减去模拟重力带来的速度影响
        speed -= gravity * Time.deltaTime;
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
        transform.position += new Vector3(0, speed, 0);
    }

    //更新小球的模拟形变
    void UpdateShape()
    {
        scale += 0.02f;
        if (scale > origScale)
        {
            scale = origScale;
        }
        Vector3 s = new Vector3(origScale, scale, origScale);
        transform.localScale = s;
    }

    void Update () {
        Bounce();
        Drop();
        UpdateShape();
    }
}
