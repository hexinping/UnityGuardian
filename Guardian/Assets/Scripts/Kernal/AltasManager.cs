using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AltasObjType
{
    Texture,
    Sprite,
}

public class AltasObj
{
    public AltasObjType type;
    public Object obj;
    public string name;

    public AltasObj(AltasObjType rType, Object rObj)
    {
        type = rType;
        name = rObj.name;
        obj = rObj;
    }
}
/*
    图集管理器
 * 
 * 暂时只支持同步加载
 *  1 使用Resources.LoadAll方法
 
 */
public class AltasManager
{


    private static AltasManager _instance = null;

    private Dictionary<string, List<AltasObj>> _cacheMap = null;

    private Dictionary<string, Dictionary<string, AltasObj>> _cacheMapDic = null;
    // Use this for initialization

    public AltasManager()
    {
        _cacheMap = new Dictionary<string, List<AltasObj>>();
        _cacheMapDic = new Dictionary<string, Dictionary<string, AltasObj>>();
    }
    static public AltasManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new AltasManager();
        }
        return _instance;
    }

    public List<AltasObj> loadPlistResource(string file)
    {
        if (!_cacheMap.ContainsKey(file))
        {
            //加载整一张图集，此方法会返回一个Object[]，里面包含了图集的纹理 Texture2D和图集下的全部Sprite
            Object[] _atlas = Resources.LoadAll("Plist/" + file);

            List<AltasObj> list = new List<AltasObj>();


            Dictionary<string, AltasObj> dict = new Dictionary<string, AltasObj>();
            _cacheMapDic.Add(file, dict);
            _cacheMap.Add(file, list);

            //图集纹理
            Texture tex = (Texture)_atlas[0];
            AltasObj altasObj = new AltasObj(AltasObjType.Texture, tex);
            dict.Add(altasObj.name, altasObj);
            list.Add(altasObj);

            //精灵
            Sprite sp = null;
            for (int i = 1; i < _atlas.Length; i++)
            {
                sp = (Sprite)_atlas[i];
                altasObj = new AltasObj(AltasObjType.Sprite, sp);
                dict.Add(altasObj.name, altasObj);
                list.Add(altasObj);
            }
        }
        return _cacheMap[file];
    }

    //删除某个plist里的资源内存
    public void removePlistResource(string file, bool isRemoveAll = false)
    {
        List<AltasObj> list = _cacheMap[file];

        if (list != null)
        {
            AltasObj altasObj = null;
            for (int i = 0; i < list.Count; i++)
            {
                altasObj = list[i];
                unloadAsset(altasObj.obj);
            }

            if (!isRemoveAll)
            {
                _cacheMap.Remove(file);
            }

            if (_cacheMapDic.ContainsKey(file))
            {
                _cacheMapDic.Remove(file);
            }
        }
    }

    //删除所有plist里的资源
    public void removeAllPlistResource()
    {
        if (_cacheMap != null)
        {
            foreach (KeyValuePair<string, List<AltasObj>> kv in _cacheMap)
            {
                string file = kv.Key;
                removePlistResource(file, true);
            }
            _cacheMap.Clear();
            _cacheMapDic.Clear();
        }

    }

    //获取单个资源
    public AltasObj getSingleResource(string file, string name)
    {
        AltasObj altasObj = null;
        if (_cacheMapDic != null)
        {
            if (_cacheMapDic.ContainsKey(file))
            {
                altasObj = _cacheMapDic[file][name];
            }

        }
        return altasObj;
    }

    //获取单个texture资源
    public Texture getSingleTextureResource(string file)
    {
        Texture tex = null;
        if (_cacheMapDic != null)
        {
            if (_cacheMapDic.ContainsKey(file))
            {
                AltasObj altasObj = _cacheMapDic[file][file];
                tex = (Texture)altasObj.obj;
            }

        }
        return tex;
    }

    //获取单个sprite资源
    public Sprite getSingleSpriteResource(string file, string name)
    {
        Sprite sp = null;
        if (_cacheMapDic != null)
        {
            if (_cacheMapDic.ContainsKey(file))
            {
                AltasObj altasObj = _cacheMapDic[file][name];
                sp = (Sprite)altasObj.obj;
            }

        }
        return sp;
    }

    public void unloadAsset(Object obj)
    {
        Resources.UnloadAsset(obj);
    }



}
