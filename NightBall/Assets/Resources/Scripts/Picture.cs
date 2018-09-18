using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture {

    GameObject picture;

    public Picture()
    {
        Data();
        picture.transform.position = new Vector3 (0, 0, -1);
    }

    public Picture(float x, float y)
    {
        Data();
        picture.transform.position = new Vector3 (x, y, -1);
    }

    public GameObject Exemplar
    {
        get { return picture; }
    }

    void Data()
    {
        picture = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Picture"));
        picture.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Background/bgPicture_" + Random.Range(1, 21));
        picture.GetComponent<SpriteRenderer>().color = NewColor();
        picture.transform.Rotate(0, 0, Random.Range(-50.0f, 50.0f));
        picture.transform.localScale += new Vector3(0.3f, 0.3f, 0);
    }

    Color NewColor()
    {
        Color color;
        int colorff;
        colorff = Random.Range(1, 5);
        switch (colorff)
        {
            case 1:
                {
                    int color88;
                    color88 = Random.Range(1, 3);
                    if (color88 == 1)
                    {
                        color = new Color(1, 0.5f, Random.Range(0.5f, 1.0f), 0.7f);
                    }
                    else
                    {
                        color = new Color(1, Random.Range(0.5f, 1.0f), 0.5f, 0.7f);
                    }
                    break;
                }
            case 2:
                {
                    int color88;
                    color88 = Random.Range(1, 3);
                    if (color88 == 1)
                    {
                        color = new Color(Random.Range(0.5f, 1.0f), 1, 0.5f, 0.7f);
                    }
                    else
                    {
                        color = new Color(0.5f, 1, Random.Range(0.5f, 1.0f), 0.7f);
                    }
                    break;
                }
            case 3:
                {
                    int color88;
                    color88 = Random.Range(1, 3);
                    if (color88 == 1)
                    {
                        color = new Color(Random.Range(0.5f, 1.0f), 0.5f, 0.7f);
                    }
                    else
                    {
                        color = new Color(0.5f, Random.Range(0.5f, 1.0f), 0.7f);
                    }
                    break;
                }
            default:
                {
                    color = new Color(1,1,1,1);
                    break;
                }
        }
        return color;
    }

}
