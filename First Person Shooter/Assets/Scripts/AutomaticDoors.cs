using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoors : MonoBehaviour {
    
    public Transform rightDoor;
    public Transform rightDoorOpened;
    public Transform rightDoorClosed;
    public Transform leftDoor;
    public Transform leftDoorOpened;
    public Transform leftDoorClosed;

    public float speed = 1.0f;

    bool isOpening;
    bool isClosing;

    Vector3 distance;
    
	// Update is called once per frame
	void Update () {
		if(isOpening)
        {
            distance = leftDoor.localPosition - leftDoorOpened.localPosition;

            if(distance.magnitude < 0.001f)
            {
                isOpening = false;
                leftDoor.localPosition = leftDoorOpened.localPosition;
                rightDoor.localPosition = rightDoorOpened.localPosition;
            }
            else
            {
                leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftDoorOpened.localPosition, speed * Time.deltaTime);
                rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightDoorOpened.localPosition, speed * Time.deltaTime);
            }
        }else if (isClosing)
        {
            distance = leftDoor.localPosition - leftDoorClosed.localPosition;

            if (distance.magnitude < 0.001f)
            {
                isClosing = false;
                leftDoor.localPosition = leftDoorClosed.localPosition;
                rightDoor.localPosition = rightDoorClosed.localPosition;
            }
            else
            {
                leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition, leftDoorClosed.localPosition, speed * Time.deltaTime);
                rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition, rightDoorClosed.localPosition, speed * Time.deltaTime);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        isOpening = true;
        isClosing = false;
    }

    private void OnTriggerStay(Collider other)
    {
        isOpening = true;
        isClosing = false;
    }

    private void OnTriggerExit(Collider other)
    {
        isOpening = false;
        isClosing = true;
    }
}
