using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMove : MonoBehaviour
{
    private Light m_light;
    private float m_time = 0;
    private bool IsBack;
    // Start is called before the first frame update
    void Start()
    {
        m_light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsBack)
            m_time += Time.deltaTime;
        else
            m_time -= Time.deltaTime;

        if (m_time >= 2)
        {
            IsBack = true;
        }
        if (m_time <= -2)
            IsBack = false;
        m_light.intensity += m_time/700;
    }
}
