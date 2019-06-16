using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CometIndicator : MonoBehaviour
{
    [SerializeField] Sprite m_targetIconOffScreen;
    [Range(0, 100)] [SerializeField] float m_edgeBuffer;
    [SerializeField] Vector3 m_targetIconScale;
    [SerializeField] bool PointTarget = true;

    bool m_outOfScreen;

    Canvas mainCanvas;
    Camera mainCamera;
    RectTransform m_icon;
    Image m_iconImage;

    void Start()
    {
        mainCamera = Camera.main;
        FindMainCanvas();
        InstainateTargetIcon();
    }

    void Update()
    {
        UpdateTargetIconPosition();
    }

    void FindMainCanvas()
    {
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (!canvas.GetComponent<BlackholeNumber>())
            {
                mainCanvas = canvas;
            }
        }
    }

    void InstainateTargetIcon()
    {
        m_icon = new GameObject().AddComponent<RectTransform>();
        m_icon.transform.SetParent(mainCanvas.transform);
        m_icon.localScale = m_targetIconScale;
        m_icon.name = name + ": OTI icon";
        m_iconImage = m_icon.gameObject.AddComponent<Image>();
        m_iconImage.sprite = m_targetIconOffScreen;
        m_iconImage.color = Color.red;
    }

    void UpdateTargetIconPosition()
    {
        Vector3 newPos = transform.position;
        newPos = mainCamera.WorldToViewportPoint(newPos);
        if (newPos.x > 1 || newPos.y > 1 || newPos.x < 0 || newPos.y < 0)
        {
            m_outOfScreen = true;
        }
        else
        {
            m_outOfScreen = false;
        }
        if (newPos.z < 0)
        {
            newPos.x = 1f - newPos.x;
            newPos.y = 1f - newPos.y;
            newPos.z = 0;
            newPos = Vector3Maxamize(newPos);
        }
        newPos = mainCamera.ViewportToScreenPoint(newPos);
        newPos.x = Mathf.Clamp(newPos.x, m_edgeBuffer, Screen.width - m_edgeBuffer);
        newPos.y = Mathf.Clamp(newPos.y, m_edgeBuffer, Screen.height - m_edgeBuffer);
        m_icon.transform.position = newPos;
        if (m_outOfScreen)
        {
            m_iconImage.enabled = true;
            if (PointTarget)
            {
                var targetPosLocal = mainCamera.transform.InverseTransformPoint(transform.position);
                var targetAngle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg - 90;
                m_icon.transform.eulerAngles = new Vector3(0, 0, targetAngle);
            }
        }
        else
        {
            m_icon.transform.eulerAngles = new Vector3(0, 0, 0);
            m_iconImage.enabled = false;
        }
    }

    Vector3 Vector3Maxamize(Vector3 vector)
    {
        Vector3 returnVector = vector;
        float max = 0;
        max = vector.x > max ? vector.x : max;
        max = vector.y > max ? vector.y : max;
        max = vector.z > max ? vector.z : max;
        returnVector /= max;
        return returnVector;
    }

    public GameObject GetIndicator()
    {
        return m_icon.gameObject;
    }
}