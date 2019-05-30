using UnityEngine;
using System.Collections;
using kernal;

public class DestroyForTime : MonoBehaviour {

	public float time;
    public bool isUsePool = false; //�Ƿ�ʹ�ö��󻺳��
    public string poolName = "";  //ʹ�ö��󻺳����Ҫ��֪���������
    private float createTime;
    private float endTime;

	void Start () {
        createTime = Time.time;
        endTime = createTime + time;
        
	}

    void onEnable()
    {
        createTime = Time.time;
        endTime = createTime + time;
    }
    void Update()
    {
        if (createTime +Time.deltaTime >= endTime)
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
