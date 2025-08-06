using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEncounter : MonoBehaviour
{
    public bool areaComplete = false;
    public Collider areaCol;
    public Collider[] areaEntrances;

    // Start is called before the first frame update
    void Start()
    {
        if(!areaCol.isTrigger)
        {
            Debug.LogWarning(areaCol.name + " collider needs to be a trigger");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void AreaComplete()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LockDoors();
        }
    }

    protected void LockDoors()
    {
        print("Doors Locked");
    }

}
