//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine.UI;

public enum eModelEnum
{
    HAND_1,
    HAND_2
}

public enum eEditMode
{
    Heading, 
    Pitch,
    Roll,
    Pan,
    Ruller
}

namespace GoogleARCore.Examples.HelloAR
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        public Vector2 startPos;
        public Vector2 direction;

        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a vertical plane.
        /// </summary>
        public GameObject AndyVerticalPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a horizontal plane.
        /// </summary>
        public GameObject AndyHorizontalPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a feature point.
        /// </summary>
        public GameObject AndyPointPrefab;

        public Text MeasureResultLabel;

        public GameObject PointIconPrefab;

        public GameObject MeasureTextPrefab;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            //List<string> list = new List<string> { "Hand1", "Hand2" };
            //var dropdown = GetComponent<Dropdown>();
            //dropdown.ClearOptions(); 
            //dropdown.AddOptions(list);
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            m_zoomInButton = GameObject.Find("ZoomInButton");
            m_zoomOutButton = GameObject.Find("ZoomOutButton");
            m_zoomInButton.GetComponent<Button>().interactable = false;
            m_zoomOutButton.GetComponent<Button>().interactable = false;

            EventManager.AddHandler(eEventEnum.Hand1Selected, new Action<object>((p_val) => {
                switchModel(eModelEnum.HAND_1);
            }));

            EventManager.AddHandler(eEventEnum.Hand2Selected, new Action<object>((p_val) => {
                switchModel(eModelEnum.HAND_2);
            }));

            EventManager.AddHandler(eEventEnum.EditModeChanged, new Action<object>((p_editMode) => {
                if(p_editMode == null)
                {
                    m_currentEditMode = eEditMode.Pan;
                }
                else
                {
                    eEditMode selectedMode = (eEditMode)p_editMode;
                    m_currentEditMode = selectedMode;
                }
            }));

            EventManager.AddHandler(eEventEnum.ExitRuller, new System.Action<object>((p_obj) =>
            {
                m_measureCounter = 0;
            }));
        }

        public void ChangeColor()
        {
            EventManager.Broadcast(eEventEnum.ChangeColor, null);
        }
   
        private void switchModel(eModelEnum p_model)
        {

            if (m_currentModel == p_model)
            {
                return;
            }

            if (p_model == eModelEnum.HAND_1)
            {
                m_currentPrefab = AndyHorizontalPlanePrefab;
            }
            else if(p_model == eModelEnum.HAND_2)
            {
                m_currentPrefab = AndyPointPrefab;
            }

            m_currentModel = p_model;

            Vector3 position = m_andyObject.transform.position;
            Quaternion rotation = m_andyObject.transform.rotation;
            Destroy(m_andyObject);
            m_andyObject = Instantiate(m_currentPrefab, position, rotation);
            setModelPosition(position);
        }
        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        ///     public GameObject projectile;

        public void Update()
        {
            _UpdateApplicationLifecycle();

            if(m_modelAdded)
            {
                if(Input.touchCount == 2)
                {
                    pinchZoom();
                    return;
                }
                // Track a single touch as a direction control.
                if (Input.touchCount > 0)
                {
                    Touch singleTouch = Input.GetTouch(0);

                    // Handle finger movements based on TouchPhase
                    switch (singleTouch.phase)
                    {
                        //When a touch has first been detected, change the message and record the starting position
                        case TouchPhase.Began:
                            // Record initial touch position.
                            startPos = singleTouch.position;
                            break;

                        //Determine if the touch is a moving touch
                        case TouchPhase.Moved:
                            // Determine direction by comparing the current touch position with the initial one
                            direction = singleTouch.position - startPos;

                            switch (m_currentEditMode)
                            {
                                case eEditMode.Roll:
                                    {
                                        if (direction.x < 0)
                                        {
                                            m_andyObject.transform.Rotate(m_andyObject.transform.rotation.x, m_andyObject.transform.rotation.y, m_andyObject.transform.rotation.z - 3);
                                        }
                                        else
                                        {
                                            m_andyObject.transform.Rotate(m_andyObject.transform.rotation.x, m_andyObject.transform.rotation.y, m_andyObject.transform.rotation.z + 3);
                                        }
                                        break;
                                    }
                                case eEditMode.Heading:
                                    {
                                        if (direction.x < 0)
                                        {
                                            m_andyObject.transform.Rotate(m_andyObject.transform.rotation.x, m_andyObject.transform.rotation.y - 3, m_andyObject.transform.rotation.z);
                                        }
                                        else
                                        {
                                            m_andyObject.transform.Rotate(m_andyObject.transform.rotation.x, m_andyObject.transform.rotation.y + 3, m_andyObject.transform.rotation.z);
                                        }
                                        break;
                                    }
                                case eEditMode.Pitch:
                                    {
                                        if (direction.y < 0)
                                        {
                                            m_andyObject.transform.Rotate(m_andyObject.transform.rotation.x + 3, m_andyObject.transform.rotation.y, m_andyObject.transform.rotation.z);
                                        }
                                        else
                                        {
                                            m_andyObject.transform.Rotate(m_andyObject.transform.rotation.x - 3, m_andyObject.transform.rotation.y, m_andyObject.transform.rotation.z);
                                        }
                                        break;
                                    }
                                case eEditMode.Pan:
                                    {
                                        m_andyObject.transform.localPosition = new Vector3(m_andyObject.transform.localPosition.x + direction.normalized.x / 100, m_andyObject.transform.localPosition.y,
                                            m_andyObject.transform.localPosition.z + direction.normalized.y / 100);
                                        break;
                                    }
                            }

                            break;

                        case TouchPhase.Ended:
                            {
                            }
                            break;
                    }
                }
                return;
            }

            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            // Should not handle input if the player is pointing on UI.
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }


            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else if (m_currentEditMode == eEditMode.Ruller)
                {
                    if(m_measureCounter++ == 0)
                    {
                        m_previousMeasurePoint = hit.Pose.position;
                        //Instantiate(PointIconPrefab, m_previousMeasurePoint, Quaternion.identity);
                    }
                    else
                    {
                        MeasureResultLabel.text = getDistance(m_previousMeasurePoint, hit.Pose.position).ToString();
                        Vector3[] points = new Vector3[2];
                        points[0] = new Vector3(m_previousMeasurePoint.x, m_previousMeasurePoint.y, m_previousMeasurePoint.z);
                        points[1] = new Vector3(hit.Pose.position.x, hit.Pose.position.y, hit.Pose.position.z);
                
                        EventManager.Broadcast(eEventEnum.DrawLine, points);
                        EventManager.Broadcast(eEventEnum.DrawMeasureText, new MeasureText(MeasureTextPrefab, getMidPoint(m_previousMeasurePoint, hit.Pose.position), 
                            getDistance(m_previousMeasurePoint, hit.Pose.position).ToString("#.00")));
                        m_previousMeasurePoint = hit.Pose.position;
                    }

                    EventManager.Broadcast(eEventEnum.AddMeasurePoint, new MeasurePoint(PointIconPrefab, hit.Pose.position));
                }
                else
                {
                    // Choose the Andy model for the Trackable that got hit.
                    GameObject prefab = AndyHorizontalPlanePrefab;

                    if (!m_modelAdded)
                    {
                        // Instantiate Andy model at the hit pose.
                        m_andyObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);
                        m_modelAdded = true;
                        m_zoomInButton.GetComponent<Button>().interactable = true;
                        m_zoomOutButton.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        setModelPosition(new Vector3(hit.Pose.position.x, hit.Pose.position.y, hit.Pose.position.z));
                    }

                    setModelCoreProperties(hit);
                }
            }
        }

        private float getDistance(Vector3 p_firstPos, Vector3 p_secondPos)
        {
            float dx = p_firstPos.x - p_secondPos.x;
            float dy = p_firstPos.y - p_secondPos.y;
            float dz = p_firstPos.z - p_secondPos.z;

            float distanceMet = (float)Math.Sqrt(dx*dx + dy*dy + dz*dz);
            return distanceMet * 100; ;
        }

        private Vector3 getMidPoint(Vector3 p_firstPoint, Vector3 p_secondPoint)
        {
            return (p_firstPoint + p_secondPoint) / 2f;
        }
        
        private void pinchZoom()
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            EventManager.Broadcast(eEventEnum.PinchZoom, deltaMagnitudeDiff * -1 / 1000);
         }

        private void setModelPosition(Vector3 p_position)
        {
            m_andyObject.transform.position = p_position;
        }

        private void setModelCoreProperties(TrackableHit p_hit)
        {
            // Compensate for the hitPose rotation facing away from the raycast (i.e.
            // camera).
            m_andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

            // Create an anchor to allow ARCore to track the hitpoint as understanding of
            // the physical world evolves.
            var anchor = p_hit.Trackable.CreateAnchor(p_hit.Pose);

            // Make Andy model a child of the anchor.
            m_andyObject.transform.parent = anchor.transform;
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }
        private bool m_modelAdded;
        private GameObject m_andyObject;
        private GameObject m_zoomInButton;
        private GameObject m_zoomOutButton;
        private GameObject m_currentPrefab;
        private GameObject m_modelPanel;
        private eModelEnum m_currentModel = eModelEnum.HAND_1;
        private Vector2 m_touchDirection;
        private Vector2 m_touchStartPos;
        private eEditMode m_currentEditMode = eEditMode.Pan;
        private int m_measureCounter;
        private Vector3 m_previousMeasurePoint;
       // public Material lineMat = new Material("Shader \"Lines/Colored Blended\" {" + "SubShader { Pass { " + "    Blend SrcAlpha OneMinusSrcAlpha " + "    ZWrite Off Cull Off Fog { Mode Off } " + "    BindChannels {" + "      Bind \"vertex\", vertex Bind \"color\", color }" + "} } }");
    }
}
