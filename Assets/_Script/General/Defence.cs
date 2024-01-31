using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defence : MonoBehaviour
{
    //这个脚本用于护盾或者伤害减免这个概念（可以被通用所以放到了general），子弹建议通过这个脚本，这个脚本再单独去执行对HP脚本的控制/扣血
    //对于怪物只有护盾|||对于屏障随着击破数提升血量|||对于玩家提升伤害减免和护盾，减免持续存在或者有一部分是玩家自身属性，还可以有一些装备提供减免属性，即他是可以变化的，需要一个接口函数去被调用
    //还有一个事件接口可以通过tag改变数值

    //声明
    public float ShieldStrength;            //护盾强度：用于直接抵伤
    public float DamageReduction;           //伤害减免：百分比减免伤害
                                            //逻辑是先过减免然后扣护盾
    
    //接口函数

    //更新护盾值和伤害减免百分比
    public void UpgradeData(float StrengthIncrease, float ReductionIncrease)
    {
        ShieldStrength += StrengthIncrease;
        DamageReduction += ReductionIncrease;
    }

    //调用这个处理外部原始伤害值
    public void DealDamage(float Damage)
    {
        float TotalDamage = CalculateTotalDamage(Damage);
        if(ShieldStrength > 0f)
        {
            float RealDamage = Mathf.Max(0f, TotalDamage - ShieldStrength);//计算真伤
            ShieldStrength = Mathf.Max(0f, ShieldStrength - TotalDamage);//计算护盾剩余
            ImplementDamage(RealDamage);
        }
        else
        {
            ImplementDamage(TotalDamage);
        }
    }

    //内部函数

    //伤害减免计算总处理伤害
    public float CalculateTotalDamage(float Damage)
    {
        return Damage * (1f - DamageReduction);
    }

    //实施最终伤害
    public void ImplementDamage(float Damage)
    {
        //这里调用真实伤害到Health
        gameObject.GetComponent<Health>().TakeDamage(Damage);
    }
}
