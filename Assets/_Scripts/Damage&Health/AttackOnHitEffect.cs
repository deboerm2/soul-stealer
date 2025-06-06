using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnHitEffect : MonoBehaviour
{
    [Tooltip("Duration of any lingering effects in seconds")]
    public float duration;
    public Effects onHitEffects;

    private Attack attack;

    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<Attack>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Health>() != null && !other.CompareTag(attack.currentTag))
        { 
            GameObject effectObject = new GameObject();
            effectObject.tag = attack.currentTag;

            effectObject.AddComponent<SphereCollider>().isTrigger = true;
            //effectObject.GetComponent<SphereCollider>().radius = 0.2f;

            effectObject.AddComponent<EffectArea>().lifetime = duration;
            effectObject.GetComponent<EffectArea>().areaEffects = onHitEffects;
            effectObject.GetComponent<EffectArea>().unaffectedTag = attack.currentTag;

            effectObject.transform.parent = other.transform;
            effectObject.transform.localPosition = Vector3.zero;
        }
    }
}
