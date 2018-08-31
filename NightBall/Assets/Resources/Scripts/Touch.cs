using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт проверки для объектов left и right на нажатие или отжатие кнопки мыши/прикосновения или отпускания на тачпаде в области триггера
public class Touch : MonoBehaviour {

    bool onTouched;

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
