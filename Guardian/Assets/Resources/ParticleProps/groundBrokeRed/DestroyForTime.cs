using UnityEngine;
using System.Collections;
using kernal;

public class DestroyForTime : MonoBehaviour {

	public float time;
    public bool isUsePool = false; //�Ƿ�ʹ�ö��󻺳��
    public string poolName = "";  //ʹ�ö��󻺳����Ҫ��֪���������
    private float playTime = 0.0f;
    private float endTime;

	void Start () {
        playTime = Time.time;
        endTime = Time.time + time;
        
	}

    void OnEnable()
    {
        playTime = Time.time;
        endTime = Time.time + time;
    }
    void Update()
    {
        playTime += Time.deltaTime;
        if (playTime >= endTime)
        {
            if (!isUsePool)
            {
                Destroy(gameObject);
            }
            else
            {
                //���뵽����صķǻ����
                if (poolName!= string.Empty)
                    PoolManager.PoolsArray[poolName].RecoverGameObjectToPools(gameObject);
            }
        }
    
    }

}
