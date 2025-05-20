using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvents : MonoBehaviour
{
    BodyTakeover bodyTakeover;
    Weapon weapon;

    private void Start()
    {
        bodyTakeover = gameObject.GetComponentInParent<BodyTakeover>();
        weapon = gameObject.GetComponentInChildren<Weapon>();
    }
    public void clearBools()
    {
        bodyTakeover.SetAnimatorParam("Light", false);
        bodyTakeover.SetAnimatorParam("Special", false);
        bodyTakeover.SetAnimatorParam("inCombo", false);
        weapon.col.enabled = true;
    }

    public void acceptAttackInput()
    {
        bodyTakeover.acceptAttackInputs = true;
        weapon.col.enabled = false;
    }

}
