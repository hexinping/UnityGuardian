using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 数据基类。所有的数据类继承这个类
 */
public class BaseMode  {

    //*****************************数据属性
    public int id;

    public float hp;
    public float maxHp;
    public float atk;  //攻击力
    public float maxAtk;
    public float magic; //魔法值
    public float maxMagic;
    public float defence; //防御力
    public float maxDefence;
    public float dexterity; //敏捷度
    public float maxDexterity;
    public float atkByPro;          //道具攻击力
    public float defenceByPro;      //道具防御力
    public float dexterityByPro;    //道具敏捷度

    public float warningDisSquare;       //警戒距离平方
    public float attackDisSquare;        //攻击距离平方

    //*****************************数据属性


    public static string c_hp               = "hp";
    public static string c_maxHp            = "maxHp";
    public static string c_atk              = "atk";
    public static string c_maxAtk           = "maxAtk";
    public static string c_magic            = "magic";
    public static string c_maxMagic         = "maxMagic";
    public static string c_defence          = "defence";
    public static string c_maxDefence       = "maxDefence";
    public static string c_dexterity        = "dexterity";
    public static string c_maxDexterity     = "maxDexterity";
    public static string c_atkByPro         = "atkByPro";
    public static string c_defenceByPro     = "defenceByPro";
    public static string c_dexterityByPro   = "dexterityByPro";


    public void update(Dictionary<string, object> values)
    {
        if (values.ContainsKey(BaseMode.c_hp))
        {
            hp = (float)values[BaseMode.c_hp];
        }

        if (values.ContainsKey(BaseMode.c_maxHp))
        {
            maxHp = (float)values[BaseMode.c_maxHp];
        }

        if (values.ContainsKey(BaseMode.c_atk))
        {
            atk = (float)values[BaseMode.c_atk];
        }

        if (values.ContainsKey(BaseMode.c_maxAtk))
        {
            maxAtk = (float)values[BaseMode.c_maxAtk];
        }

        if (values.ContainsKey(BaseMode.c_magic))
        {
            magic = (float)values[BaseMode.c_magic];
        }

        if (values.ContainsKey(BaseMode.c_maxMagic))
        {
            maxMagic = (float)values[BaseMode.c_maxMagic];
        }

        if (values.ContainsKey(BaseMode.c_defence))
        {
            defence = (float)values[BaseMode.c_defence];
        }

        if (values.ContainsKey(BaseMode.c_maxDefence))
        {
            maxDefence = (float)values[BaseMode.c_maxDefence];
        }

        if (values.ContainsKey(BaseMode.c_dexterity))
        {
            dexterity = (float)values[BaseMode.c_dexterity];
        }

        if (values.ContainsKey(BaseMode.c_maxDexterity))
        {
            maxDexterity = (float)values[BaseMode.c_maxDexterity];
        }

        if (values.ContainsKey(BaseMode.c_atkByPro))
        {
            atkByPro = (float)values[BaseMode.c_atkByPro];
        }

        if (values.ContainsKey(BaseMode.c_defenceByPro))
        {
            defenceByPro = (float)values[BaseMode.c_defenceByPro];
        }

        if (values.ContainsKey(BaseMode.c_dexterityByPro))
        {
            dexterityByPro = (float)values[BaseMode.c_dexterityByPro];
        }

    }

    public float getHpValue()
    {
        float result = hp;
        return result;
    }

}
