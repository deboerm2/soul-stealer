using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectArea : MonoBehaviour
{
    [System.Serializable]
    public struct Effects
    {
        public bool slow;
        public bool damage;
    }

    public float lifetime;
    public Effects areaEffects;

    private HashSet<GameObject> affectedObjects = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //0 will mean an infinite lifetime. useful for effects tied to the room/environment rather than an attack
        if(lifetime != 0)
            StartCoroutine(Lifespan(lifetime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ApplyEffects(GameObject gameobj)
    {

    }
    void RemoveEffects(GameObject gameobj)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //affectedObjects.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        //affectedObjects.Remove(other.gameObject);
    }

    IEnumerator Lifespan(float _lifespan)
    {
        yield return new WaitForSeconds(_lifespan);
        foreach(GameObject obj in affectedObjects)
        {
            //clear effects
        }
        Destroy(gameObject);
    }
}
