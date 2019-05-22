
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace kernal
{
    public class Pools : MonoBehaviour
    {
        [HideInInspector]
        public Transform ThisGameObjectPosition;                                   //���������Ϸ����λ��              
        public List<PoolOption> PoolOptionArrayLib = new List<PoolOption>();       //����ģ����ء���������
        public bool IsUsedTime = false;                                            //�Ƿ��á�ʱ�����

        void Awake()
        {
            PoolManager.Add(this);                                                 //���롰��ģ���ϳء�������
            ThisGameObjectPosition = transform;
            //Ԥ����
            PreLoadGameObject();
        }

        void Start()
        {
            //��ʼʱ�������
            if (IsUsedTime)
            {
                InvokeRepeating("ProcessGameObject_NameTime", 1F, 10F);
            }
        }


        /*
         ʱ�������
            ��Ҫҵ���߼�:
            1>: ÿ���10���֣�����������ʹ�õĻ״̬��Ϸ�����ʱ�����ȥ10�롣
            2>:  ���ÿ���״̬����Ϸ�������Ƶ�ʱ������С�ڵ���0����������״̬��
            3>:  ���½���״̬����Ϸ���󣬻��Ԥ���趨�Ĵ��ʱ��д��������Ƶ�ʱ����С�
         */

        void ProcessGameObject_NameTime()
        {
            //ѭ����Ϊ����ġ����ࡱ����
            for (int i = 0; i < PoolOptionArrayLib.Count; i++)
            {
                PoolOption opt = this.PoolOptionArrayLib[i];
                //��������ʹ�õĻ״̬��Ϸ�����ʱ�����ȥ10��
                //���ÿ���״̬����Ϸ�������Ƶ�ʱ������С�ڵ���0����������״̬
                opt.AllActiveGameObjectTimeSubtraction();
            }//for_end    
        }

        // Ԥ����
        public void PreLoadGameObject()
        {
            for (int i = 0; i < this.PoolOptionArrayLib.Count; i++)
            {              //����ģ������
                PoolOption opt = this.PoolOptionArrayLib[i];                       //����ģ������
                for (int j = opt.totalCount; j < opt.IntPreLoadNumber; j++)
                {
                    GameObject obj = opt.PreLoad(opt.Prefab, Vector3.zero, Quaternion.identity);
                    //����Ԥ���ص���Ϸ����涨ΪPool��������Ϸ������Ӷ���
                    obj.transform.parent = ThisGameObjectPosition;
                }
            }
        }



        /*
         �õ���Ϸ���󣬴ӻ�����У�����ģ�����ϣ�
            1�� ��ָ����Ԥ�衱���Լ��Ļ�����м���һ�����Ҽ����Լ�������е�"���ü����"��
            2�� Ȼ���ٽ���һ���ض����Ҽ���Ԥ�裬�ټ����Լ��Ļ�����еġ����ü���ء��С�
         */

        public GameObject GetGameObjectByPool(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            GameObject obj = null;

            //ѭ����Ϊ����ġ����ࡱ����
            for (int i = 0; i < PoolOptionArrayLib.Count; i++)
            {
                PoolOption opt = this.PoolOptionArrayLib[i];
                if (opt.Prefab == prefab)
                {
                    //����ָ����Ԥ�衱
                    obj = opt.Active(pos, rot);
                    if (obj == null) return null;

                    //���м������Ϸ��������Ǳ������ҿն�����Ӷ���
                    if (obj.transform.parent != ThisGameObjectPosition)
                    {
                        obj.transform.parent = ThisGameObjectPosition;
                    }
                }
            }//for_end

            return obj;
        }//BirthGameObject_end

        // �ջ���Ϸ���󣨡���ģ�����ϣ�
        public void RecoverGameObjectToPools(GameObject instance)
        {
            for (int i = 0; i < this.PoolOptionArrayLib.Count; i++)
            {
                PoolOption opt = this.PoolOptionArrayLib[i];
                //����Լ���ÿһ�ࡰ�ء����Ƿ����ָ���ġ�Ԥ�衱����
                if (opt.ActiveGameObjectArray.Contains(instance))
                {
                    if (instance.transform.parent != ThisGameObjectPosition)
                        instance.transform.parent = ThisGameObjectPosition;
                    //�ض����ء��������ָ���Ķ���
                    opt.Deactive(instance);
                }
            }
        }

        // �������õĶ��󣨡���ģ�����ϣ�
        public void DestoryUnused()
        {
            for (int i = 0; i < this.PoolOptionArrayLib.Count; i++)
            {
                PoolOption opt = this.PoolOptionArrayLib[i];
                opt.ClearUpUnused();
            }
        }

        // ����ָ����������Ϸ����
        public void DestoryPrefabCount(GameObject prefab, int count)
        {
            for (int i = 0; i < this.PoolOptionArrayLib.Count; i++)
            {
                PoolOption opt = this.PoolOptionArrayLib[i];
                if (opt.Prefab == prefab)
                {
                    opt.DestoryCount(count);
                    return;
                }
            }

        }

        // �����ű������ص���Ϸ��������ʱ��������м�����ȫ������
        public void OnDestroy()
        {
            //ֹͣʱ����ļ��
            if (IsUsedTime)
            {
                CancelInvoke("ProcessGameObject_NameTime");
            }
            for (int i = 0; i < this.PoolOptionArrayLib.Count; i++)
            {
                PoolOption opt = this.PoolOptionArrayLib[i];
                opt.ClearAllArray();
            }
        }

    }//Pool.cs_end


    // ��Ϸ�������ͣ�����������������ģ�ز������� ���ܣ� ����ջء�Ԥ���صȡ�
    [System.Serializable]
    public class PoolOption
    {
        public GameObject Prefab;                                                  //�洢�ġ�Ԥ�衱
        public int IntPreLoadNumber = 0;                                           //��ʼ��������
        public int IntAutoDeactiveGameObjectByTime = 30;                            //��ʱ���Զ�������Ϸ����

        [HideInInspector]
        public List<GameObject> ActiveGameObjectArray = new List<GameObject>();    //�ʹ�õ���Ϸ���󼯺�
        [HideInInspector]
        public List<GameObject> InactiveGameObjectArray = new List<GameObject>();   //�ǻ״̬�����ã�����Ϸ���󼯺�
        private int _Index = 0;



        /*
             Ԥ����
                <param name="prefab">��Ԥ�衱��</param>
                <param name="positon">λ��</param>
                <param name="rotation">��ת</param>
         
        */
        internal GameObject PreLoad(GameObject prefab, Vector3 positon, Quaternion rotation)
        {
            GameObject obj = null;

            if (prefab)
            {
                obj = Object.Instantiate(prefab, positon, rotation) as GameObject;
                Rename(obj);
                obj.SetActive(false);                                              //���÷ǻ״̬
                //���뵽���ǻ��Ϸ���󡱼����С�
                InactiveGameObjectArray.Add(obj);
            }
            return obj;
        }


        /*
           ������Ϸ����
               <param name="pos">λ��</param>
               <param name="rot">��ת</param>
         */
        internal GameObject Active(Vector3 pos, Quaternion rot)
        {
            GameObject obj;

            if (InactiveGameObjectArray.Count != 0)
            {
                //�ӡ��ǻ��Ϸ���ϡ�������ȡ���±�Ϊ0����Ϸ����
                obj = InactiveGameObjectArray[0];
                //�ӡ��ǻ��Ϸ���ϡ��������Ƴ��±�Ϊ0����Ϸ����
                InactiveGameObjectArray.RemoveAt(0);
            }
            else
            {
                //���ء���û�ж���Ķ���������µĶ���
                obj = Object.Instantiate(Prefab, pos, rot) as GameObject;
                //�µĶ���������ơ���ʽ��������
                Rename(obj);
            }
            //����ķ�λ����
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            //�¶�����ʽ���롰��ء������С�
            ActiveGameObjectArray.Add(obj);
            obj.SetActive(true);

            return obj;
        }


        //������Ϸ����
        internal void Deactive(GameObject obj)
        {
            ActiveGameObjectArray.Remove(obj);
            InactiveGameObjectArray.Add(obj);
            obj.SetActive(false);
        }

        // ͳ���������ء������ж��������
        internal int totalCount
        {
            get
            {
                int count = 0;
                count += this.ActiveGameObjectArray.Count;
                count += this.InactiveGameObjectArray.Count;
                return count;
            }
        }


        // ȫ����ռ��ϣ��������ء���
        internal void ClearAllArray()
        {
            ActiveGameObjectArray.Clear();
            InactiveGameObjectArray.Clear();
        }


        // ����ɾ�����С��ǻ�����������е���Ϸ����
        internal void ClearUpUnused()
        {
            foreach (GameObject obj in InactiveGameObjectArray)
            {
                Object.Destroy(obj);
            }

            InactiveGameObjectArray.Clear();
        }


         /*
             ��Ϸ����������
              ���²�������Ϸ������ͳһ��ʽ����Ŀ��������ʱ���������
         */
 
        private void Rename(GameObject instance)
        {
            instance.name += (_Index + 1).ToString("#000");
            //��Ϸ�����Զ����ã�ʱ���  [Adding]
            instance.name = IntAutoDeactiveGameObjectByTime + "@" + instance.name;
            _Index++;
        }

        // ɾ�����ǻ�����������е�һ����ָ����������
        internal void DestoryCount(int count)
        {
            if (count > InactiveGameObjectArray.Count)
            {
                ClearUpUnused();
                return;
            }
            for (int i = InactiveGameObjectArray.Count - 1; i >= InactiveGameObjectArray.Count - count; i--)
            {

                Object.Destroy(InactiveGameObjectArray[i]);
            }
            InactiveGameObjectArray.RemoveRange(InactiveGameObjectArray.Count - count, count);
        }

        /*
            �ص�������ʱ�������
              ���ܣ�������Ϸ�������ʱ�䵹��ʱ����ʱ��С��������С��ǻ�����������У���:��ʱ���Զ�������Ϸ����
         */
        internal void AllActiveGameObjectTimeSubtraction()
        {
            for (int i = 0; i < ActiveGameObjectArray.Count; i++)
            {
                string strHead = null;
                string strTail = null;
                int intTimeInfo = 0;
                GameObject goActiveObj = null;

                goActiveObj = ActiveGameObjectArray[i];
               
                if (goActiveObj!=null)
                {               
                    //�õ�ÿ�������ʱ���
                    string[] strArray = goActiveObj.name.Split('@');
                    strHead = strArray[0];
                    strTail = strArray[1];

                    //ʱ���-10 ����
                    intTimeInfo = System.Convert.ToInt32(strHead);
                    if (intTimeInfo >= 10)
                    {
                        strHead = (intTimeInfo - 10).ToString();
                    }
                    else if (intTimeInfo <= 0)
                    {
                        //��Ϸ�����Զ�ת�����
                        goActiveObj.name = IntAutoDeactiveGameObjectByTime.ToString() + "@" + strTail;
                        this.Deactive(goActiveObj);
                        continue;
                    }
                    //ʱ�����������
                    goActiveObj.name = strHead + '@' + strTail;
                }
            }
        }

    }//PoolOption.cs_end


    /// <summary>
    /// �ڲ��ࣺ ��ʱ��
    /// </summary>
    //[System.Serializable]
    public class PoolTimeObject
    {
        public GameObject instance;
        public float time;
    }//PoolTimeObject.cs_end
}












