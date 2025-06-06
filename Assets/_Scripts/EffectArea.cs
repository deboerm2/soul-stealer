using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectArea : MonoBehaviour
{
    public float lifetime;
    public Effects areaEffects;

    //[HideInInspector]
    //currently used so enemies don't get slowed by enemies
    public string unaffectedTag;

    private HashSet<EffectHandler> affectedObjects = new HashSet<EffectHandler>();

    // Start is called before the first frame update
    void Start()
    {
        //0 will mean an infinite lifetime. useful for effects tied to the room/environment rather than an attack
        if(lifetime != 0)
            StartCoroutine(Lifespan(lifetime));
    }

    private void OnTriggerEnter(Collider other)
    {
        //has EffectHandler and is not tagged with unaffected tag
        if(other.gameObject.GetComponent<EffectHandler>() != null && !other.CompareTag(unaffectedTag))
            affectedObjects.Add(other.gameObject.GetComponent<EffectHandler>());
    }
    private void OnTriggerStay(Collider other)
    {
        foreach(EffectHandler affected in affectedObjects)
        {
            affected.ActivateEffects(areaEffects);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        EffectHandler otherHandler = other.gameObject.GetComponent<EffectHandler>();
        if (otherHandler != null)
            otherHandler.RemoveEffects();
        affectedObjects.Remove(otherHandler);
    }
    IEnumerator Lifespan(float _lifespan)
    {
        yield return new WaitForSeconds(_lifespan);
        foreach(EffectHandler affected in affectedObjects)
        {
            if (affected == null)
                continue;

            //affectedObjects.Remove(affected);
            affected.RemoveEffects();
        }
        Destroy(gameObject);
    }
}
