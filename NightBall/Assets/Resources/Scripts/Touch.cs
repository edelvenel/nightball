using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour {

    public bool onTouched;

    void Update()
    {
        
    }

    public bool IsTouched
    {
        get { return onTouched; }
    }

    private void OnMouseDown()
    {
        onTouched = true;
    }

    void OnMouseUp()
    {
        onTouched = false;
    }

}
