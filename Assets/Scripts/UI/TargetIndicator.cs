using UnityEngine;
using UnityEngine.UI;
public class TargetIndicator : MonoBehaviour
{
    private Camera mainCamera;
    private RectTransform m_icon;
    private Image iconImage;
    private Canvas mainCanvas;
    private Vector3 cameraOffsetUp;
    private Vector3 cameraOffsetRight;
    private Vector3 cameraOffsetForward;
    public Sprite TargetIconOnScreen;
    public Sprite TargetIconOffScreen;
    [Space]
    [Range(0, 100)]
    public float EdgeBuffer;
    public Vector3 TargetIconScale = new Vector3(1, 1, 1);
    [Space]
    public bool PointTarget = true;
    public bool ShowDebugLines;
    //Indicates if the object is out of the screen
    private bool m_outOfScreen;

    void Start()
    {
        //prevent script from running if it's on the local player
        if (gameObject.transform.parent != GameController.LocalPlayer)
        {
            gameObject.SetActive(false);
            return;
        }

        mainCamera = Camera.main;
        mainCanvas = FindObjectOfType<Canvas>();
        Debug.Assert((mainCanvas != null), "There needs to be a Canvas object in the scene for the OTI to display");
        InstainateTargetIcon();
    }
    void Update()
    {
        if (ShowDebugLines)
            DrawDebugLines();
        UpdateTargetIconPosition();
    }
    private void InstainateTargetIcon()
    {
        m_icon = new GameObject().AddComponent<RectTransform>();
        m_icon.transform.SetParent(mainCanvas.transform);
        m_icon.localScale = TargetIconScale;
        m_icon.name = name + ": OTI icon";
        iconImage = m_icon.gameObject.AddComponent<Image>();
        iconImage.sprite = TargetIconOnScreen;
    }
    private void UpdateTargetIconPosition()
    {
        Vector3 newPos = transform.position;
        newPos = mainCamera.WorldToViewportPoint(newPos);
        //Simple check if the target object is out of the screen or inside
        if (newPos.x > 1 || newPos.y > 1 || newPos.x < 0 || newPos.y < 0)
            m_outOfScreen = true;
        else
            m_outOfScreen = false;
        if (newPos.z < 0)
        {
            newPos.x = 1f - newPos.x;
            newPos.y = 1f - newPos.y;
            newPos.z = 0;
            newPos = Vector3Maxamize(newPos);
        }
        newPos = mainCamera.ViewportToScreenPoint(newPos);
        newPos.x = Mathf.Clamp(newPos.x, EdgeBuffer, Screen.width - EdgeBuffer);
        newPos.y = Mathf.Clamp(newPos.y, EdgeBuffer, Screen.height - EdgeBuffer);
        //NaN killing validation #FixTheSymptomNotTheCause
        if (!float.IsNaN(newPos.x) && !float.IsNaN(newPos.y) && !float.IsNaN(newPos.z))
        {
            m_icon.transform.position = newPos;

        }
        //Operations if the object is out of the screen
        if (m_outOfScreen)
        {
            //Show the target off screen icon
            iconImage.sprite = TargetIconOffScreen;
            if (PointTarget)
            {
                //Rotate the sprite towards the target object
                Vector3 targetPosLocal = mainCamera.transform.InverseTransformPoint(transform.position);
                float targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg;
                //Apply rotation
                m_icon.transform.eulerAngles = new Vector3(0, 0, targetAngle);
            }

        }
        else
        {
            //Reset rotation to zero and swap the sprite to the "on screen" one
            m_icon.transform.eulerAngles = new Vector3(0, 0, 0);
            iconImage.sprite = TargetIconOnScreen;
        }

    }
    public void DrawDebugLines()
    {
        Vector3 directionFromCamera = transform.position - mainCamera.transform.position;
        Vector3 cameraForwad = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        Vector3 cameraUp = mainCamera.transform.up;
        cameraForwad *= Vector3.Dot(cameraForwad, directionFromCamera);
        cameraRight *= Vector3.Dot(cameraRight, directionFromCamera);
        cameraUp *= Vector3.Dot(cameraUp, directionFromCamera);
        Debug.DrawRay(mainCamera.transform.position, directionFromCamera, Color.magenta);
        Vector3 forwardPlaneCenter = mainCamera.transform.position + cameraForwad;
        Debug.DrawLine(mainCamera.transform.position, forwardPlaneCenter, Color.blue);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraUp, Color.green);
        Debug.DrawLine(forwardPlaneCenter, forwardPlaneCenter + cameraRight, Color.red);
    }
    public Vector3 Vector3Maxamize(Vector3 vector)
    {
        Vector3 returnVector = vector;
        float max = 0;
        max = vector.x > max ? vector.x : max;
        max = vector.y > max ? vector.y : max;
        max = vector.z > max ? vector.z : max;
        returnVector /= max;
        return returnVector;
    }
}