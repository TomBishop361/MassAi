using System.Reflection;
using UnityEngine;

public class FootSoldier : Unit
{
    // play noises/ anims for footsoldier

    internal override void Attack()
    {
        
        base.Attack();

    }

    internal override void Perish()
    {
        //Custom noise 
        base.Perish();
        
    }


}
