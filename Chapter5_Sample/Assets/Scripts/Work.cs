using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Work : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(Move());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject() == false)
            {               
                GetComponent<Animator>().SetTrigger("click");
            }

            
        }
            
    }


    
}
