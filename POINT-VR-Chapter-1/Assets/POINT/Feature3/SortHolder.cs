using System;
using System.Collections.Generic;
using UnityEngine;

namespace POINT.Feature3
{
    public class SortHolder : MonoBehaviour
    {
        public SnapRing snapRingPrefab;
        List<SnapRing> snapRings = new List<SnapRing>();
        public int snapRingCount = 5;
        public float Padding = 0.2f;
        private float snapRingZWidth = 1f;
        [SerializeField] XRHardwareController hardwareController;
        [SerializeField] GameObject NextTaskButton;
        public void Awake()
        {
            // from child object find all snap rings
            snapRingZWidth = snapRingPrefab.GetComponent<BoxCollider>().size.z;
            float currentZ = 0;
            // make sure the snapRingPrefab is set active
            snapRingPrefab.gameObject.SetActive(true);
            for (int i = 0; i < snapRingCount; i++)
            {
                SnapRing snapRing = Instantiate(snapRingPrefab, transform);
                snapRing.transform.localPosition = new Vector3(0, 0, currentZ);
                snapRings.Add(snapRing);
                currentZ += snapRingZWidth + Padding;
            }
            snapRingPrefab.gameObject.SetActive(false);
            
            
        }
        [ContextMenu("Sort")]
        public bool CheckSortOrder()
        {
            List<float> masses= new List<float>();
            // order should be ascend 
            // for correct order, change the color to green
            // for incorrect order, change the color to red
            foreach (SnapRing snapRing in snapRings)
            {
                if (snapRing.collidingObject == null)
                {
                    // debug warning 
                    // only vibrate hand if it is apk use Macro
                    # if UNITY_ANDROID && !UNITY_EDITOR
                    hardwareController.VibrateHand();
                    #endif
                    return false;
                }
                masses.Add(snapRing.collidingObject.mass);
            }
            
            //sort the list
            masses.Sort();
            bool isCorrectOrder = true;
            // check the mass follow the order
            for (int i = 0; i < masses.Count; i++)
            {
                if (masses[i] != snapRings[i].collidingObject.mass)
                {
                    isCorrectOrder = false;
                    // set the color to red by changing the material
                    snapRings[i].GetComponent<MeshRenderer>().material.color = Color.red;
                }
                else
                {
                    // set the color to green by changing the material
                    snapRings[i].GetComponent<MeshRenderer>().material.color = Color.green;
                }
            }

            return isCorrectOrder;
        }

        public void Test()
        {
           bool result= CheckSortOrder();
              if (result)
              {
                NextTaskButton.SetActive(true);
              }
        }
    }
}