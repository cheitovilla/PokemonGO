using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndThrow : MonoBehaviour
{
    bool dragging = false;
    float distance;
    public float throwSpeed;
    public float archspeed;
    public float speed;

    // Start is called before the first frame update
    public void Start()
    {

    }

    public void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
    }

    public void OnMouseUp()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().velocity += this.transform.forward * throwSpeed;
        this.GetComponent<Rigidbody>().velocity += this.transform.up * archspeed;
        dragging = false;
    }


  

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(this.transform.position, rayPoint, speed * Time.deltaTime);
        }
    }
}
