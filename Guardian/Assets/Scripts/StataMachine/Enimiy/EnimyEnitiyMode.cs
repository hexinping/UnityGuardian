using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnimyEnitiyMode : BaseMode{

    public float hp;
    public float maxHp;
    public float atk;  //攻击力
    public float maxAtk;
    public float defence; //防御力
    public float maxDefence;
    public float dexterity; //敏捷度
    public float maxDexterity;

    public float atkByPro;          //道具攻击力
    public float defenceByPro;      //道具防御力
    public float dexterityByPro;    //道具敏捷度

    public EnimyEnitiyMode()
    {

        maxAtk = 1.0f;
        atk = maxAtk;
        maxHp = 20.0f;
        hp = maxHp;
        maxDefence = 0.0f;
        defence = maxDefence;
        maxDexterity = 5.0f;
        dexterity = maxDexterity;

        atkByPro = 0.0f;
        defenceByPro = 0.0f;
        dexterityByPro = 0.0f;
    }

    public float countDamage(BaseEnitity target)
    {
        float resultAtk = getAtkValue();
        float resultDefence = target.getDefenceValue();
        float damage = resultAtk - resultDefence;
        if (damage <= 0)
        {
            damage = 0;
        }
        return damage;
    }


    public float getAtkValue()
    {
        float result = atk + atkByPro;
        return result;
    }

    public float getDefenceValue()
    {
        float result = defence + defenceByPro;
        return result;
    }

    public void updateHp(float fHp)
    {
        hp = fHp;
    }


    public float getHpValue()
    {
        float result = hp;
        return result;
    }

}
