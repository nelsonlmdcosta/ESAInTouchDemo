using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [Header("Input Controls")]
    [SerializeField] private float leftBorderPercentage = 0.10f;
    [SerializeField] private float rightBorderPercentage = 0.90f;

    [SerializeField] private float swipeMagnitude = 10;
    [SerializeField] private float zoomMagnitude = 10;

    [SerializeField] private float rotateTouchSensitivity = 5;
    [SerializeField] private float zoomTouchSensitivity = 100;

    [SerializeField] private float menuClosingSwipeStrength = 10;
    [SerializeField] private float pressElegibilitySensitivity = 10;

    private float zoomFingerDistanceLastFrame = 0;

    [Header("Camera")]
    [SerializeField] private CameraController cam = null;

    [Header("UI")]
    [SerializeField] private Transform leftPanel = null;
    [SerializeField] private Transform rightPanel = null;

    [SerializeField] private Transform leftPanelInDock = null;
    [SerializeField] private Transform leftPanelOutDock = null;

    [SerializeField] private Transform rightPanelInDock = null;
    [SerializeField] private Transform rightPanelOutDock = null;

    [SerializeField] private float lerpTime = 0;
    private float startTime;

    [Header("Satellite UI Refs")]
    [SerializeField] private GameObject parentObject;
    [SerializeField] private Text missionText;
    [SerializeField] private Text satelliteDetailsText;

    private enum LerpState { None, LerpingLeftPanelOut, LerpingLeftPanelIn, LerpingRightPanelOut, LerpingRightPanelIn }
    private LerpState lerpState = LerpState.None;

    private enum MenuState { Closed, LeftMenuOpen, RightMenuOpen }
    private MenuState menuState = MenuState.Closed;

    private Vector2 initialPress = Vector2.zero;
    private bool elegibleForPress = true;

    void Update ()
    {
        switch (menuState)
        {
            case MenuState.Closed:
                HandleWorldViewTouchControls();
                return;
            case MenuState.LeftMenuOpen:
                if (lerpState != LerpState.None) // Handle Lerping
                {
                    float normalizedTime = 0;
                    if (lerpState == LerpState.LerpingLeftPanelIn)
                        normalizedTime = Mathf.Clamp01(1 - ((startTime + lerpTime) - Time.time) / lerpTime);
                    else if (lerpState == LerpState.LerpingLeftPanelOut)
                        normalizedTime = Mathf.Clamp01(1 - ((startTime + lerpTime) - Time.time) / lerpTime);
                    LerpMenu(lerpState, leftPanel, normalizedTime);

                    if (normalizedTime == 1)
                    {
                        if (lerpState == LerpState.LerpingLeftPanelOut || lerpState == LerpState.LerpingRightPanelOut)
                            menuState = MenuState.Closed;
                        lerpState = LerpState.None;
                    }
                }
                else // Handle Any Menu Interactions
                {
                    HandleMenuTouchControls();
                }
                return;
            case MenuState.RightMenuOpen:
                if (lerpState != LerpState.None) // Handle Lerping
                {
                    float normalizedTime = 0;
                    if (lerpState == LerpState.LerpingRightPanelIn)
                        normalizedTime = Mathf.Clamp01(1 - ((startTime + lerpTime) - Time.time) / lerpTime);
                    else if(lerpState == LerpState.LerpingRightPanelOut)
                        normalizedTime = Mathf.Clamp01(1 - ((startTime + lerpTime) - Time.time) / lerpTime);
                    LerpMenu(lerpState, rightPanel, normalizedTime);

                    if (normalizedTime == 1)
                    {
                        if (lerpState == LerpState.LerpingLeftPanelOut || lerpState == LerpState.LerpingRightPanelOut)
                            menuState = MenuState.Closed;
                        lerpState = LerpState.None;
                    }
                }
                else // Handle Any Menu Interactions
                {
                    HandleMenuTouchControls();
                }
                return;
            default:
                Debug.LogError("SwitchCase Isnt Meant To Get Here PANIC! >:O");
                break;
        }
	}

    bool HandleWorldViewTouchControls()
    {
        Touch[] touches = Input.touches;
        if (touches.Length > 0) // Make Sure There Are Touches On Screen
        {
            // Zoom Camera In Out
            if (touches.Length >= 2)
            {
                float distance = Vector2.Distance(touches[0].position, touches[1].position);
                // Fingers Moved Away From Each OTher
                if (distance > zoomFingerDistanceLastFrame + zoomMagnitude)
                {
                    cam.UpdateZoomDistance(-distance / zoomTouchSensitivity);
                }
                else if (distance < zoomFingerDistanceLastFrame - zoomMagnitude) // Fingers Moved Closer
                {
                    cam.UpdateZoomDistance(distance / zoomTouchSensitivity);
                }
                // Set it up for next frame
                zoomFingerDistanceLastFrame = distance;
                return true;
            }

            // If Just A Press
            if (touches[0].phase == TouchPhase.Began)
            {
                elegibleForPress = true;
                initialPress = touches[0].position;
            }
            else if ( Vector2.Distance(initialPress, touches[0].position) > pressElegibilitySensitivity && touches[0].position.y > Screen.height * 0.20f) // Remves the elegibility of it being a press if over a threshold
            {
                elegibleForPress = false;
            }
            else if (touches[0].phase == TouchPhase.Ended && elegibleForPress && touches[0].position.y > Screen.height * 0.20f)
            {
                Ray ray = Camera.main.ScreenPointToRay(touches[0].position);
                RaycastHit hit;
                LayerMask layerMask = (1 << LayerMask.NameToLayer("Targetable"));
                if (Physics.Raycast(ray, out hit, 1000f, layerMask))
                {
                    if (hit.collider != null)
                    {
                        cam.SetParameters(hit.collider.GetComponent<CameraParameters>());
                        if (hit.collider.CompareTag("Satellite"))
                        {
                            SatelliteDetails details = hit.collider.GetComponent<SatelliteDetails>();
                            missionText.text = details.MissionText;
                            satelliteDetailsText.text = details.DetailsText;
                            parentObject.SetActive(true);
                        }
                        else parentObject.SetActive(false);
                    }
                }
            }

            // Open Left UI Panel
            if (touches[0].position.x <= Screen.width * leftBorderPercentage) // If Finger Is On The Left Side Of The Screen
            {
                if (touches[0].deltaPosition.magnitude > swipeMagnitude && touches[0].deltaPosition.x > 0) // If Swipe Was Strong Enough Open Menu
                {
                    if (menuState == MenuState.Closed) // If Menu Is Closed Open It
                    {
                        lerpState = LerpState.LerpingLeftPanelIn;
                        menuState = MenuState.LeftMenuOpen;
                        startTime = Time.time;
                        return true;
                    }
                }
                return false;
            }

            // Open Right UI Panel
            if (touches[0].position.x > Screen.width * rightBorderPercentage) // If Finger Is On The Left Side Of The Screen
            {
                if (touches[0].deltaPosition.magnitude > swipeMagnitude && touches[0].deltaPosition.x < 0) // If Swipe Was Strong Enough Open Menu
                {
                    if (menuState == MenuState.Closed) // If Menu Is Closed Open It
                    {
                        lerpState = LerpState.LerpingRightPanelIn;
                        menuState = MenuState.RightMenuOpen;
                        startTime = Time.time;
                        return true;
                    }
                }
                return false;
            }

            // Rotate Camera Around Object
            if (touches[0].position.x > Screen.width * leftBorderPercentage && touches[0].position.x < Screen.width * rightBorderPercentage && touches[0].position.y > Screen.height * 0.20f) // If Finger Is Inside External Bounds Screen
            {
                if (touches[0].deltaPosition.magnitude > swipeMagnitude) // If Swipe Was Strong Enough Open Menu
                {
                    // Add Angles To Rotation
                    Vector2 delta = touches[0].deltaPosition / rotateTouchSensitivity;
                    cam.UpdateRotation( delta );
                }
                return true;
            }
        }
        return false;
    }
    void HandleMenuTouchControls()
    {
        Touch[] touches = Input.touches;
        if (touches.Length > 0) // Make Sure There Are Touches On Screen
        {
            if (Mathf.Abs(touches[0].deltaPosition.x) > menuClosingSwipeStrength)
            {
                if (menuState == MenuState.LeftMenuOpen)
                {
                    lerpState = LerpState.LerpingLeftPanelOut;
                    startTime = Time.time;
                }
                else if (menuState == MenuState.RightMenuOpen)
                {
                    lerpState = LerpState.LerpingRightPanelOut;
                    startTime = Time.time;
                }
            }
        }
    }

    void LerpMenu(LerpState lerpState, Transform target, float time)
    {
        if ( lerpState == LerpState.LerpingLeftPanelIn )
        {
            target.position = Vector3.Lerp( leftPanelOutDock.position, leftPanelInDock.position, time );
            return;
        }
        if (lerpState == LerpState.LerpingLeftPanelOut)
        {
            target.position = Vector3.Lerp(leftPanelInDock.position, leftPanelOutDock.position, time);
            return;
        }

        if (lerpState == LerpState.LerpingRightPanelIn)
        {
            target.position = Vector3.Lerp(rightPanelOutDock.position, rightPanelInDock.position, time);
            return;
        }
        if (lerpState == LerpState.LerpingRightPanelOut)
        {
            target.position = Vector3.Lerp(rightPanelInDock.position, rightPanelOutDock.position, time);
            return;
        }
    }
}
