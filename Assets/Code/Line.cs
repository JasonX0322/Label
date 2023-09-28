using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    GameObject startPos;
    GameObject endPos;
    RectTransform rect;
    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitLine(GameObject start,GameObject end)
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();
        startPos =start;
        endPos=end;
    }

    // Update is called once per frame
    void Update()
    {
        if(startPos!=null)
        {
            float dis=Vector3.Distance(endPos.transform.position,startPos.transform.position);
            rect.sizeDelta = new Vector2(0.05f, dis);

            transform.position = startPos.transform.position;

            double angle = Mathf.Atan2(endPos.transform.position.y-startPos.transform.position.y, endPos.transform.position.x - startPos.transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, (float)angle + 270);
            
        }
    }

    public void HideLine()
    {
        rect.sizeDelta = new Vector2(5, 0);
    }
}
