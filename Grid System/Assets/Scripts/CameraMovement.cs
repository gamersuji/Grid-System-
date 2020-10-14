using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private float movePosY;
    private float movePosX;
    [SerializeField]private float dampness;
    [SerializeField] private float multiplier;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {




        //if (Input.GetAxisRaw("Horizontal") > 0)
        //{

        //}

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            movePosY += Input.GetAxisRaw("Vertical") * multiplier;
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            movePosY += Input.GetAxisRaw("Vertical") * multiplier;
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            movePosY = 0;
        }

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            movePosX += Input.GetAxisRaw("Horizontal") * multiplier;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            movePosX += Input.GetAxisRaw("Horizontal") * multiplier;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            movePosX = 0;
        }

        Vector3 movePos = new Vector2(movePosX, movePosY);
        transform.Translate(movePos / dampness);


    }
}
