
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace kernal
{
    public class PoolManager : MonoBehaviour
    {
        //“缓冲池”集合
        public static Dictionary<string, Pools> PoolsArray = new Dictionary<string, Pools>();

        // 加入“池”
        public static void Add(Pools pool)
        {
            if (PoolsArray.ContainsKey(pool.name)) return;
            PoolsArray.Add(pool.name, pool);
        }

        // 删除不用的
        public static void DestroyAllInactive()
        {
            foreach (KeyValuePair<string, Pools> keyValue in PoolsArray)
            {
                keyValue.Value.DestoryUnused();
            }
        }

        // 清空“池”
        void OnDestroy()
        {
            PoolsArray.Clear();
        }
    }
}
