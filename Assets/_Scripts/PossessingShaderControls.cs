using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessingShaderControls : MonoBehaviour
{
    // Start is called before the first frame update
    public SkinnedMeshRenderer[] shaderMeshes;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach(SkinnedMeshRenderer skinMesh in shaderMeshes)
        {

            if(skinMesh.materials[1].HasProperty("_isPossessable"))
            {
                skinMesh.materials[1].SetInt("_isPossessable", GetComponentInParent<BodyTakeover>().isPossesable ? 1 : 0);
            }
            if(skinMesh.materials[1].HasProperty("_isTarget"))
            {
                skinMesh.materials[1].SetInt("_isTarget", transform.parent.parent.gameObject == FindObjectOfType<BodyPossession>().takeoverTarget ? 1 : 0);
            }
        }

    }
}
