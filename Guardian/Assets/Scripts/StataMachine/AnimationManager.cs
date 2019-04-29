using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager {

    private static AnimationManager _instance;
    private Dictionary<string, Dictionary<string, AnimationClip>> _dictAnimClip;

    private AnimationManager()
    {
        _dictAnimClip = new Dictionary<string, Dictionary<string, AnimationClip>>();
    }

    static public AnimationManager getInstance()
    {
        if (_instance == null)
        {
            _instance = new AnimationManager();
        }
        return _instance;
    
    }

    //public void Add
}
