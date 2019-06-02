﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnimyEnitiyMode : BaseMode{

    public string file;
    public string filePre;
    public string type;
    public EnimyEnitiyMode()
    {

        maxAtk = Random.Range(1,5);
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

        moveSpeed = Random.Range(1, 10); ;

        warningDisSquare = Random.Range(25, 100);
        attackDisSquare = Random.Range(9, 25);
        type = "warrior";
        filePre = "Models/Enemys/Skeleton_Pack/Prefabs(chibi)/";
        file = filePre + type + "/skeleton_";
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



    public void update(Dictionary<string, object> values)
    {
        base.update(values);

        //专有数据更新

    }


}
