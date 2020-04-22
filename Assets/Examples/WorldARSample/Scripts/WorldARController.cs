namespace Preview
{
    using UnityEngine;
    using System.Collections.Generic;
    using HuaweiARUnitySDK;
    using System.Collections;
    using System;
    using HuaweiARInternal;
    using Common;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

    public class WorldARController : MonoBehaviour
    {
        [Tooltip("plane visualizer")]
        public GameObject planePrefabs;

        [Tooltip("plane label visualizer")]
        public GameObject planeLabelPrefabs;

        [Tooltip("green logo visualizer")]
        public GameObject arDiscoveryLogoPlanePrefabs;

        [Tooltip("blue logo visualizer")]
        public GameObject arDiscoveryLogoPointPrefabs;

        private List<ARAnchor> addedAnchors = new List<ARAnchor>();
        private List<ARPlane> newPlanes = new List<ARPlane>();
        public bool isOpen = false;
        public GameObject scrollMenu;
        public GameObject openImage;

        public GameObject zyds;
        public string currentSelect = "default";

        public void OnRefresh()
        {
            Debug.Log(addedAnchors.Count);
            for(int i = 0; i < addedAnchors.Count; i++)
            {
                ARAnchor anchor = addedAnchors[i];
                anchor.Detach();
            }
            addedAnchors.Clear();
        }

        public void OnOpenClick()
        {
            isOpen = !isOpen;
            scrollMenu.SetActive(isOpen);

            Sprite downImage = Resources.Load("image/down", typeof(Sprite)) as Sprite;
            Sprite upImage = Resources.Load("image/up", typeof(Sprite)) as Sprite;
            if (isOpen)
            {
                openImage.GetComponent<Image>().sprite = downImage;
            }
            else
            {
                openImage.GetComponent<Image>().sprite = upImage;
            }
        }

        public void Update()
        {
            //修改
            if (addedAnchors.Count < 1) 
            {
                _DrawPlane();
            }
            Touch touch;
            if (ARFrame.GetTrackingState() != ARTrackable.TrackingState.TRACKING
                || Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began || IsPointerOverUIObject())
            {
                
            }
            else
            {
                if (addedAnchors.Count < 1)
                {
                    _DrawARLogo(touch);
                }
            }
        }

        private void _DrawPlane()
        {
            newPlanes.Clear();
            ARFrame.GetTrackables<ARPlane>(newPlanes,ARTrackableQueryFilter.NEW);
            for (int i = 0; i < newPlanes.Count; i++)
            {
                // 地面才渲染 0    2为墙面
                Debug.Log("平面类型" + (int)newPlanes[i].GetARPlaneType());
                if ((int)newPlanes[i].GetARPlaneType() != 0)
                {
                    return;
                }
                GameObject planeObject = Instantiate(planePrefabs, Vector3.zero, Quaternion.identity, transform);
                planeObject.GetComponent<TrackedPlaneVisualizer>().Initialize(newPlanes[i]);


                GameObject planeLabelObject = Instantiate(planeLabelPrefabs, Vector3.zero, Quaternion.identity, transform);
                planeLabelObject.GetComponent<PlaneLabelVisualizer>().Initialize(newPlanes[i]);
            }
        }

        private void _DrawARLogo(Touch touch)
        {
            List<ARHitResult> hitResults = ARFrame.HitTest(touch);
            ARHitResult hitResult = null;
            ARTrackable trackable = null;
            Boolean hasHitFlag = false;
            ARDebug.LogInfo("_DrawARLogo hitResults count {0}", hitResults.Count);
            Debug.Log("_DrawARLogo hitResults count " +  hitResults.Count);
            foreach (ARHitResult singleHit in hitResults)
            {
                trackable = singleHit.GetTrackable();
                ARDebug.LogInfo("_DrawARLogo GetTrackable {0}", singleHit.GetTrackable());
                Debug.Log("_DrawARLogo GetTrackable " + singleHit.GetTrackable());
                if ((trackable is ARPlane && ((ARPlane)trackable).IsPoseInPolygon(singleHit.HitPose)) ||
                    (trackable is ARPoint))
                {
                    hitResult = singleHit;
                    hasHitFlag = true;
                    if (trackable is ARPlane)
                    {
                        break;
                    }                 
                }
            }

            if (hasHitFlag != true)
            {
                ARDebug.LogInfo("_DrawARLogo can't hit!");
                Debug.Log("_DrawARLogo can't hit!");
                return;
            }

            //if (addedAnchors.Count > 2)
            //{
            //    ARAnchor toRemove = addedAnchors[0];
            //    toRemove.Detach();
            //    addedAnchors.RemoveAt(0);
            //}

            GameObject prefab;
            trackable = hitResult.GetTrackable();

            if (trackable is ARPlane)
            {
                Debug.Log("当前模型:" + currentSelect);
                switch (currentSelect)
                {
                    case "zyds": prefab = (GameObject)Resources.Load("prefabs/zyds");break;
                    case "eiffel": prefab = (GameObject)Resources.Load("prefabs/eiffel"); break;
                    default: prefab = arDiscoveryLogoPlanePrefabs; break;
                }
            }
            else
            {
                return;
                //prefab = arDiscoveryLogoPointPrefabs;
            }

            ARAnchor anchor = hitResult.CreateAnchor();
            Debug.Log("创建锚点");
            var logoObject = Instantiate(prefab, anchor.GetPose().position, anchor.GetPose().rotation);
            Debug.Log("初始化模型");
            logoObject.GetComponent<ARDiscoveryLogoVisualizer>().Initialize(anchor);
            Debug.Log("初始化模型2");
            addedAnchors.Add(anchor);
            Debug.Log("新增锚点,数量 " + addedAnchors.Count);
        }

        private static bool IsPointerOverUIObject()
      {
         if (EventSystem.current == null)
              return false;
         PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
         eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
 
         List<RaycastResult> results = new List<RaycastResult>();
         EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
 
         return results.Count > 0;
      }

        public void SetZYDS()
        {
            Debug.Log("改变prefabs ZYDS");
            currentSelect = "zyds";
        }

        public void SetEiffel()
        {
            Debug.Log("改变prefabs Eiffel");
            currentSelect = "eiffel";
        }
    }
}
