using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using kernal;

public class ArcherEnimyEnitity : EnimyEnitity
{
    private GameObject _prefabArrow;
    public ArcherEnimyEnitity()
    {
        hpHeight = 100;
    }
    override public void initModeData()
    {
        _mode = new EnimyEnitiyMode();
        _mode.type = "archer";
        objName = "archer_green";
        _mode.file = _mode.filePre + _mode.type + "/skeleton_" + objName;
        intDatas();
    }

    override public void initBufferPoolPrefab()
    {
        base.initBufferPoolPrefab();
        _prefabArrow = (GameObject)ResourcesManager.getInstance().getResouce(ResourceType.Prefab, "ParticleProps/arrow", rootView._name, true, false); 
    }

    override public void intDatas()
    {
        _mode.maxAtk = 1;
        _mode.atk = _mode.maxAtk;
        _mode.maxHp = 30.0f;
        _mode.hp = _mode.maxHp;
        _mode.maxDefence = 0.0f;
        _mode.defence = _mode.maxDefence;
        _mode.warningDisSquare = 100;
        _mode.attackDisSquare = 81;

        _mode.moveSpeed = 3.0f;
    }
    //不同模型的动画帧事件不一样 必须重载
    override public void addAinimainEvents()
    {

        List<int> attack1List = new List<int>();
        attack1List.Add(35);
        _animationEventDict[GlobalParams.anim_ennimy3_normalAttack] = attack1List;

        List<int> hurtList = new List<int>();
        hurtList.Add(20);
        _animationEventDict[GlobalParams.anim_ennimy3_hurt] = hurtList;
    }

    //不同模型的动画名称不一样 必须重载
    override public void addAnimationNames()
    {
        //这里一定要根据状态的枚举值依次添加
        _animationNameList.Add(GlobalParams.anim_ennimy3_idle);
        _animationNameList.Add(GlobalParams.anim_ennimy3_run);
        _animationNameList.Add(GlobalParams.anim_ennimy3_death);
        _animationNameList.Add(GlobalParams.anim_ennimy3_normalAttack);
        _animationNameList.Add(GlobalParams.anim_ennimy3_hurt);
    }

    override public void onDestory()
    {
      
        cObjList.Clear();
        cObjList.Add("armor");
        cObjList.Add("eyes");
        cObjList.Add("Skeletonl_base");
        cObjList.Add("bow");

        texList.Clear();
        texList.Add("archer/skel_archer_col_green");
        texList.Add("grunt/base_skeleton_col");
        texList.Add("grunt/base_skeleton_col");
        texList.Add("archer/skel_archer_col_red");

        base.onDestory();

    }

    override public void eventCallBack(BaseEnitity eniity, string animationName, bool isMove = false, Vector3 targetPos = default(Vector3))
    {
        //Debug.Log(GetType() + "testEvent======成功回调========" + GlobalParams.totalTime + " 当前帧数：" + GlobalParams.frameCount);

        if (isHurt)
        {
            //受伤事件
            isHurt = false;
        }
        else if (isAttacking)
        {
            if (attackTarget != null)
            {
                
                //attackTargetHurt(attackTarget);
                //attackTarget.updateHP();
                Vector3 startPos = selfTransform.position;
                startPos.y += 1.0f;

                targetPos = targetTransform.position;
                targetPos.y += 1.0f;

  
                float speed = 10.0f;
                playHitEffect(startPos, targetPos, _prefabArrow, GlobalParams.BulletPool, true, speed);
                float distance = System.Math.Abs(Vector3.Distance(targetPos, startPos));
                float moveTime = distance / speed;
                DelayCall delayCall = new DelayCall(moveTime, GlobalParams.frameCount, arrowEventCallBack, this, "", false, targetPos);
                GlobalParams.addDelayCall(delayCall);
            }


        }
        _animator.speed = 1.0f;
    }

    private void playHitEffect(Vector3 startPos, Vector3 endPos, GameObject prefabObj, string poolName, bool isMove = false, float moveSpeed = 0)
    {
        GameObject effObj = createEffect(startPos, prefabObj, poolName);
        effObj.transform.position = startPos;
        if (isMove)
        {
            iTween.MoveTo(effObj, iTween.Hash(
              "position", endPos,
              "easetype", iTween.EaseType.easeInSine,
              "speed", moveSpeed,
              "oncomplete", "arrowMoveEndCallBack"

           ));
        }

    }

    public void arrowEventCallBack(BaseEnitity eniity, string animationName, bool isMove = false, Vector3 targetPos = default(Vector3))
    {
        if (attackTarget != null && !attackTarget.isDead)
        {
            //当前目标的位置
            Vector3 newTargetPos = targetTransform.position;
            newTargetPos.y += 1.0f;

            //箭射出之前目标的位置
            Vector3 oldTargetPos = targetPos;
            float dis = (newTargetPos - oldTargetPos).sqrMagnitude;
            if (dis < 0.01)
            {
                attackTargetHurt(attackTarget);
                attackTarget.updateHP();
            }

            
        }
       
        _animator.speed = 1.0f;
    }

}
