using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezeChecker : MonoBehaviour
{
    Color c = new Color(0.0f, 0.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float r = c.r + (5.0f / 255.0f);
        float g = c.g + (1.0f / 255.0f);
        float b = c.b + (1.0f / 255.0f);
        if (r > 1.0f) r = 0.0f;
        if (g > 1.0f) g = 0.0f;
        if (b > 1.0f) b = 0.0f;
        c = new Color(r, g, b, 1.0f);
        gameObject.GetComponent<SpriteRenderer>().color = c;
    }
}
