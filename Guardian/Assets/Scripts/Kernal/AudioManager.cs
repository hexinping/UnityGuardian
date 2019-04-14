/***
 * 
 *  核心层： 音频管理类
 * 
 *  功能：   项目中音频剪辑统一管理。
 * 
 * 
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;                                    //泛型集合命名空间


/*
两种调用方法：
 1 直接把音频加入到内存集合中（使用了缓存机制），使用名字调用，AudioManager.Playxxxx("AudioName")
 2 直接播放音频剪辑 AudioClip，AudioManager.Playxxxx(AudioClip) 
*/

public class AudioManager : MonoBehaviour
{
    public AudioClip[] AudioClipArray;                         //预加载剪辑数组
    private  float AudioAllVolumns = 1F;                       // 音量

    private Dictionary<string, AudioClip> _AudioClipDict;      //音频库
    private Dictionary<string, AudioSource> _AudioSourceDict;  //音频源字典


    private static AudioManager _instance;

    static public AudioManager getInstance()
    {
        return _instance;
    }


    void Awake()
    {
        _instance = this;

        //音频库加载
        _AudioClipDict = new Dictionary<string, AudioClip>();

        _AudioSourceDict = new Dictionary<string, AudioSource>();
        foreach (AudioClip audioClip in AudioClipArray)
        {
            _AudioClipDict.Add(audioClip.name, audioClip);

            AudioSource audioSource = gameObject.AddComponent<AudioSource>() as AudioSource;
            audioSource.clip = audioClip;
            audioSource.playOnAwake = false;
            audioSource.volume = 1.0f;
            audioSource.loop = false;
            _AudioSourceDict.Add(audioClip.name, audioSource);

        }
    
        //从数据持久化中得到音量数值
        if (PlayerPrefs.GetFloat("AudioAllVolumns") >= 0)
        {
            AudioAllVolumns = PlayerPrefs.GetFloat("AudioAllVolumns");
            setAllVoume(AudioAllVolumns);
        }
        
    }//Start_end

    //设置所有音源的音量
    public  void setAllVoume(float volume)
    {
        foreach (KeyValuePair<string, AudioSource> pair in _AudioSourceDict)
        {
            //Debug.Log(pair.Key + " " + pair.Value);
            AudioSource source = pair.Value;
            source.volume = volume;
        }
    }

    //设置单个音源的音量
    public  void setClipVolum(AudioClip clip, float volume)
    {
        string clipName = clip.name;
        setClipVolum(clipName ,volume);

    }

    public  void setClipVolum(string clipName, float volume)
    {
        if(_AudioSourceDict.ContainsKey(clipName))
        {
            AudioSource source = _AudioSourceDict[clipName];
            source.volume = volume;
        }

    }

    public  void playMusic(AudioClip audioClip, bool isLoop = true, float volume = 1.0f)
    {
        if(audioClip == null) return;

        string name = audioClip.name;
        AudioSource souce;
        if(_AudioSourceDict.ContainsKey(name))
        {
            souce = _AudioSourceDict[name];
        }
        else
        {
            //加入缓存
            _AudioClipDict.Add(name, audioClip);
            souce = gameObject.AddComponent<AudioSource>() as AudioSource;
            souce.playOnAwake = false;
            _AudioSourceDict.Add(name, souce);
            
        }
        souce.clip = audioClip;
        souce.loop = isLoop;
        souce.volume = volume;
        souce.Play();
        
    }

    //使用这个方法一定要提前缓存过
    public  void playMusic(string name, bool isLoop = true, float volume = 1.0f)
    {
        //判断是否提前缓存
        AudioSource souce;
        if(_AudioSourceDict.ContainsKey(name))
        {
            souce = _AudioSourceDict[name];
            souce.loop = isLoop;
            souce.volume = volume;
            souce.Play();
        }
    }

    public  void playSoundEffect(AudioClip audioClip, bool isLoop = true, float volume = 1.0f)
    {
        if(audioClip == null) return;

        string name = audioClip.name;
        AudioSource souce;
        if(_AudioSourceDict.ContainsKey(name))
        {
            souce = _AudioSourceDict[name];
        }
        else
        {

            //加入缓存
            _AudioClipDict.Add(name, audioClip);
            souce = gameObject.AddComponent<AudioSource>() as AudioSource;;
            souce.playOnAwake = false;
            _AudioSourceDict.Add(name, souce);
            
        }
        souce.clip = audioClip;
        souce.loop = isLoop;
        souce.volume = volume;
        souce.Play();
    }

    //使用这个方法一定要提前缓存过
    public  void playSoundEffect(string name, bool isLoop = true, float volume = 1.0f)
    {
        //判断是否提前缓存
        AudioSource souce;
        if(_AudioSourceDict.ContainsKey(name))
        {
            souce = _AudioSourceDict[name];
            souce.loop = isLoop;
            souce.volume = volume;
            souce.Play();
        }
    }

    //保存音量大小到本地
    public void SetPrefsVolumns(float volumns)
    {
        AudioAllVolumns = volumns;
        //数据持久化
        PlayerPrefs.SetFloat("AudioAllVolumns", volumns);
    }


    //删除音源组件
    public void destoryClip(string name)
    {
        if(_AudioSourceDict.ContainsKey(name))
        {
            AudioSource souce = _AudioSourceDict[name];
            AudioClip clip = _AudioClipDict[name];

            //删除组件
            DestroyImmediate(souce,true);
            
            _AudioSourceDict.Remove(name);
            _AudioClipDict.Remove(name);
        
        }
    }

    public void destoryClip(AudioClip clip)
    {   
        string name = clip.name;

        if(_AudioSourceDict.ContainsKey(name))
        {
            AudioSource souce = _AudioSourceDict[name];
            //删除组件
            DestroyImmediate(souce,true);

            _AudioSourceDict.Remove(name);
            _AudioClipDict.Remove(name);
            
        }
    }

    //停止播放
    public void stopPlay(string name)
    {
        if(_AudioSourceDict.ContainsKey(name))
        {
            AudioSource souce = _AudioSourceDict[name];
            souce.Stop();
        }
    }

    public void stopPlay(AudioClip clip)
    {
        string name = clip.name;
        stopPlay(name);
    }

    //暂停播放
    public void pausePlay(string name)
    {
        if(_AudioSourceDict.ContainsKey(name))
        {
            AudioSource souce = _AudioSourceDict[name];
            souce.Pause();
        }

    }

    public void pausePlay(AudioClip clip)
    {
        string name = clip.name;
        pausePlay(name);

    }

    //恢复播放
    public void resumePlay(string name)
    {
        if(_AudioSourceDict.ContainsKey(name))
        {
            AudioSource souce = _AudioSourceDict[name];
            souce.UnPause();
        }

    }

    public void resumePlay(AudioClip clip)
    {
        string name = clip.name;
        resumePlay(name);

    }


}//Class_end

