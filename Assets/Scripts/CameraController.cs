using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManagement
{
    public class CameraController : MonoBehaviour
    {
        public Transform trackTarget;
        public Transform cameraObject;
        [SerializeField] private Vector3 theoreticalPlayerLocation;
        [SerializeField] private Vector2 movementArea;
        [SerializeField] private Vector2 trackingArea;
        [SerializeField] private float trackSpeed;

        Vector3 toTrackPos;

        private void FixedUpdate()
        {
            if (trackTarget == null)
                return;

            CalculateTrackPosition(trackTarget.position);
            MoveCamera();
        }

        private void MoveCamera()
        {
            cameraObject.position = Vector3.Lerp(cameraObject.position, toTrackPos, trackSpeed);
        }

        private void CalculateTrackPosition(Vector3 targetToTrack)
        {
            float trackXPercent = (targetToTrack.x / trackingArea.x) * 100f;
            float trackZPercent = (targetToTrack.z / trackingArea.y) * 100f;

            float toTrackX = (movementArea.x / 100f) * trackXPercent;
            float toTrackZ = (movementArea.y / 100f) * trackZPercent;
            toTrackX = Mathf.Clamp(toTrackX, -movementArea.x / 2, movementArea.x / 2);
            toTrackZ = Mathf.Clamp(toTrackZ, -movementArea.y / 2, movementArea.y / 2);
            toTrackPos = Vector3.Lerp(toTrackPos, transform.position + new Vector3(toTrackX, 0f, toTrackZ), trackSpeed);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, new Vector3(movementArea.x, 0f, movementArea.y));

            if (trackTarget != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(new Vector3(transform.position.x, trackTarget.position.y, transform.position.z), new Vector3(trackingArea.x, 0f, trackingArea.y));

                CalculateTrackPosition(trackTarget.position);

                Gizmos.DrawWireSphere(toTrackPos, 0.5f);
            } else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(new Vector3(theoreticalPlayerLocation.x, theoreticalPlayerLocation.y, theoreticalPlayerLocation.z), new Vector3(trackingArea.x, 0f, trackingArea.y));

                CalculateTrackPosition(theoreticalPlayerLocation);

                Gizmos.DrawWireSphere(toTrackPos, 0.5f);
            }
        }
    }
}