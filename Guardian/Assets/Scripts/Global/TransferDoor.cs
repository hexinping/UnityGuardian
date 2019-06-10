using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferDoor : MonoBehaviour
{

    public Transform target;
    private RotationDistortEffect _rotationDistort;
    private GameObject player;
    private HpFollow hp;

    private float time = 1.0f;
    private float totalTime = 3.0f; //传送门效果总时间
    private float endTime;
    private bool isOver = false;
    private bool isStart = false;

    void OnEnable()
    {
        GameObject mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        _rotationDistort = mainCamera.GetComponent<RotationDistortEffect>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Player"))
        {
            if (isStart) return;

            player = GameObject.FindGameObjectWithTag("Player");

            GameObject hpLayer = GameObject.Find("_Manager/_ViewManager/_RootLayer/_HpLayer");
            GameObject playerHP = hpLayer.transform.Find("PlayerHp").gameObject;
            //开启传送
            _rotationDistort.enabled = true;
            endTime = GlobalParams.totalTime + time;
            isStart = true;
            //hp = playerHP.GetComponent<HpFollow>();
            //hp.hideSlider();
        }

    }

    void Update()
    {
        if (!isStart) return;
        if (isOver) return;
        if (GlobalParams.totalTime >= endTime)
        {
            isOver = true;
            //主角到到达目的位
            if (target)
            {
                GlobalParams.isPause = true;
                player.transform.position = target.position;
                this.Invoke("showHp", totalTime - time + 0.5f);
            }

           
        }
    }

    void showHp()
    {
        //hp.showSlider();
        _rotationDistort.enabled = false;
        GlobalParams.isPause = false;
        Destroy(gameObject);
    }
}
