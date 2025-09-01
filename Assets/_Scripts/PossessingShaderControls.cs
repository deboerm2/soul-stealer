using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossessingShaderControls : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer[] shaderMeshes;
    private GameObject parentOBJ;
    void Start()
    {
        parentOBJ = gameObject;
        //while there is a parent object and current parent does not have body takeover
        while(parentOBJ.transform.parent != null && !parentOBJ.GetComponent<BodyTakeover>())
        {
            parentOBJ = parentOBJ.transform.parent.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Renderer rend in shaderMeshes)
        {

            if(rend.materials[1].HasProperty("_isPossessable"))
            {
                rend.materials[1].SetInt("_isPossessable", GetComponentInParent<BodyTakeover>().isPossesable ? 1 : 0);
            }
            if(rend.materials[1].HasProperty("_isTarget") && FindObjectOfType<BodyPossession>())
            {
                rend.materials[1].SetInt("_isTarget", parentOBJ == FindObjectOfType<BodyPossession>().takeoverTarget ? 1 : 0);
            }
        }

    }
}
