using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningPlane : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    [SerializeField] private Image _image;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0;
        _image.fillAmount = 0;
        StartCoroutine(WaitClose());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _image.fillAmount += Time.deltaTime;
        canvasGroup.alpha += Time.deltaTime;
    }


    IEnumerator WaitClose() 
    {
        yield return new WaitForSeconds(10f);
        gameObject.SetActive(false);
    }
}
