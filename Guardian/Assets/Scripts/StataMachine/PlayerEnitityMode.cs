using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnitityMode : BaseMode
{

    public string file;
    public float atk;  //攻击力
    public float maxHp;

    public PlayerEnitityMode()
    {

        atk = 5.0f;
        maxHp = 20.0f;
        file = "Models/SwordsMan/GreateWarrior";
    }
}
