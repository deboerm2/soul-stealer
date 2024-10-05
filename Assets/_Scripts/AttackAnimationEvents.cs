using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvents : MonoBehaviour
{
    BodyTakeover bodyTakeover;

    private void Start()
    {
        bodyTakeover = gameObject.GetComponentInParent<BodyTakeover>();
    }
    public void clearBools()
    {
        bodyTakeover.SetAnimatorParam("Light", false);
        bodyTakeover.SetAnimatorParam("Heavy", false);
        bodyTakeover.SetAnimatorParam("inCombo", false);
        print("TEST");
    }

    public void acceptAttackInput()
    {
        bodyTakeover.acceptAttackInputs = true;
    }
}
