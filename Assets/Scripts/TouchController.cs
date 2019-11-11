using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public float longPressDuration;
    public float tapRadiusPercent;

    public float swipeMinInputTime;

    public float swipeUpXMaxThresholdPercent;
    public float swipeUpYMinThresholdPercent;

    public float swipeRightXMinThresholdPercent;
    public float swipeRightYMaxThresholdPercent;

    public float swipeDownXMaxThresholdPercent;
    public float swipeDownYMinThresholdPercent;

    public bool longPressDetected = false;
    public Vector2 startPosition;
    public Vector2 endPosition;
    public float pressedTimer;

    private bool swipeRight;
    private bool swipeDown;
    private bool tap;
    private bool longPress;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pressedTimer = 0;
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mouseDelta = (Vector2)Input.mousePosition - startPosition;
            pressedTimer += Time.deltaTime;

            if (!longPressDetected && pressedTimer > longPressDuration && mouseDelta.magnitude < tapRadiusPercent * Screen.width)
            {
                longPress = true;
                longPressDetected = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            longPressDetected = false;
            endPosition = Input.mousePosition;
            Vector2 mouseDelta = endPosition - startPosition;

            // Detect SwipeDown
            if (Mathf.Abs(mouseDelta.x) < swipeDownXMaxThresholdPercent * Screen.width
                && mouseDelta.y < -(swipeDownYMinThresholdPercent * Screen.height)
                && pressedTimer < swipeMinInputTime)
            {
                swipeDown = true;
            }

            // Detect SwipeRight
            if (mouseDelta.x > swipeRightXMinThresholdPercent * Screen.width
             && Mathf.Abs(mouseDelta.y) < swipeRightYMaxThresholdPercent * Screen.height
             && pressedTimer < swipeMinInputTime)
            {
                swipeRight = true;
            }

            // Detect SwipeUp
            if (Mathf.Abs(mouseDelta.x) < swipeDownXMaxThresholdPercent * Screen.width
                && mouseDelta.y > swipeDownYMinThresholdPercent * Screen.height
                && pressedTimer < swipeMinInputTime)
            {
            }

            // Detect Tap
            if (pressedTimer < longPressDuration && mouseDelta.magnitude < tapRadiusPercent * Screen.width)
            {
                tap = true;
            }
        }
    }

    public bool GetSwipeRight()
    {
        if (swipeRight)
        {
            swipeRight = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetSwipeDown()
    {
        if (swipeDown)
        {
            swipeDown = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetTap()
    {
        if (tap)
        {
            tap = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool GetLongPress()
    {
        if (longPress)
        {
            longPress = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
