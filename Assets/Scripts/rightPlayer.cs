using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rightPlayer : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    float topScreen = 11f;
    float bottomScreen = -11f;
    float maxY = 0;
    float minY = 0;

    float height;
    Vector2 startPosition = new Vector2 (18.7f,0);

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = (Vector3)startPosition;
        height = transform.GetComponent<Renderer>().bounds.size.y;
    }
    // Update is called once per frame
    void Update()
    {
        checkInput();
    }
    void checkInput()
    {
        maxY = transform.localPosition.y + height / 2;
        minY = transform.localPosition.y - height / 2;

        // Move up
        if (Input.GetKey (KeyCode.UpArrow))
        {
            if (maxY >= topScreen)
            {
                transform.localPosition = new Vector3 (transform.localPosition.x, topScreen - height / 2, transform.localPosition.z);
            }
            else
            {
                transform.localPosition += Vector3.up * moveSpeed * Time.deltaTime;
            }
        }
        // Move down
        else if (Input.GetKey (KeyCode.DownArrow))
        {
            if (minY <= bottomScreen)
            {
                transform.localPosition = new Vector3 (transform.localPosition.x, bottomScreen + height / 2, transform.localPosition.z);
            }
            else
            {
                transform.localPosition += Vector3.down * moveSpeed * Time.deltaTime;
            }
        }
    }
    public void IncreaseSpeed()
    {
        moveSpeed += 1f;
    }
    public void UpdateHeight()
    {
        height = transform.GetComponent<Renderer>().bounds.size.y;
    }
}
