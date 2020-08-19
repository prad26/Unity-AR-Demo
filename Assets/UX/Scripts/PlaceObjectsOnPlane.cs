using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObjectsOnPlane : MonoBehaviour
{
    [SerializeField]
    bool m_SnapToMesh;

    public bool snapToMesh
    {
        get => m_SnapToMesh;
        set => m_SnapToMesh = value;
    }
    
    [SerializeField]
    GameObject m_ReticlePrefab;

    public GameObject reticlePrefab
    {
        get => m_ReticlePrefab;
        set => m_ReticlePrefab = value;
    }

    [SerializeField]
    bool m_DistanceScale;

    public bool distanceScale
    {
        get => m_DistanceScale;
        set => m_DistanceScale = value;
    }

    [SerializeField]
    Transform m_CameraTransform;

    public Transform cameraTransform
    {
        get => m_CameraTransform;
        set => m_CameraTransform = value;
    }

    GameObject m_SpawnedReticle;
    CenterScreenHelper m_CenterScreen;
    TrackableType m_RaycastMask;
    float m_CurrentDistance;
    float m_CurrentNormalizedDistance;

    const float k_MinScaleDistance = 0.0f;
    const float k_MaxScaleDistance = 1.0f;
    const float k_ScaleMod = 0.75f;

    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    private int spawnedObject = 0;

    private Pose hitPose;

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    
    [SerializeField]
    bool m_CanReposition = true;

    public bool canReposition
    {
        get => m_CanReposition;
        set => m_CanReposition = value;
    }

    void Start()
    {
        m_CenterScreen = CenterScreenHelper.Instance;
        if (m_SnapToMesh)
        {
            m_RaycastMask = TrackableType.PlaneEstimated;
        }
        else
        {
            m_RaycastMask = TrackableType.PlaneWithinPolygon;
        }

        m_SpawnedReticle = Instantiate(m_ReticlePrefab);
        m_SpawnedReticle.SetActive(false);
    }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (m_RaycastManager.Raycast(m_CenterScreen.GetCenterScreen(), s_Hits, m_RaycastMask))
        {
            hitPose = s_Hits[0].pose;
            
            if(spawnedObject!=1)
            {
                m_SpawnedReticle.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                m_SpawnedReticle.SetActive(true);
                if (m_DistanceScale)
                {
                    m_CurrentDistance = Vector3.Distance(m_SpawnedReticle.transform.position, m_CameraTransform.position);
                    m_CurrentNormalizedDistance = ((Mathf.Abs(m_CurrentDistance - k_MinScaleDistance)) / (k_MaxScaleDistance - k_MinScaleDistance))+k_ScaleMod;
                    m_SpawnedReticle.transform.localScale = new Vector3(m_CurrentNormalizedDistance, m_CurrentNormalizedDistance, m_CurrentNormalizedDistance);
                }
            }
            
            if (Input.touchCount>0 && spawnedObject!=1)
            {
                spawnedObject = 1;
                m_PlacedPrefab.SetActive(true);
                m_PlacedPrefab.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
                m_SpawnedReticle.SetActive(false);
            }
            else if (m_CanReposition && Input.touchCount>0)
            {
               Touch touch = Input.GetTouch(0);
               if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
               {
                   hitPose = s_Hits[0].pose;
                   m_PlacedPrefab.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
               }
            }
            
            if (onPlacedObject != null)
            {
                onPlacedObject();
            }
        }
    } 

}
