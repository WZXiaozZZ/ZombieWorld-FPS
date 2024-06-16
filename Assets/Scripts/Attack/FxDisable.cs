using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxDisable : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(WaitDisable());
    }

    IEnumerator WaitDisable() 
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
