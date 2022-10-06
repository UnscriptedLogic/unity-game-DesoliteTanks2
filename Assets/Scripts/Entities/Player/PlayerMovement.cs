using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class PlayerMovement : MonoBehaviour, IMoveable, IListensToGameState
    {
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody rb;

        [SerializeField] private VFXSettings movingVFX;

        private Vector3 moveDirection;

        private void Update()
        {
            GetInput();
            Move();
        }

        public void Move()
        {
            Vector3 moveVelocity = moveDirection * speed;
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

            if (moveDirection.magnitude > 0)
            {
                transform.forward = moveDirection;
                if (!movingVFX.isAudioPlaying)
                {
                    movingVFX.PlayVFX(transform.position, Quaternion.identity, transform);
                }
                else
                {
                    movingVFX.audioSource.volume = Mathf.Lerp(movingVFX.audioSource.volume, movingVFX.volume, Time.deltaTime * 5f);
                }
            }
            else
            {
                if (movingVFX.isAudioPlaying)
                {
                    movingVFX.audioSource.volume = Mathf.Lerp(movingVFX.audioSource.volume, 0.0025f, Time.deltaTime * 5f);
                }
            }
        }

        public void GetInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveDirection = Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveDirection = Vector3.back;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveDirection = Vector3.left;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                moveDirection = Vector3.right;
            }
            else
            {
                moveDirection = Vector3.zero;
            }
        }

        public void OnGameStateChanged(bool won)
        {
            enabled = false;
        }
    }
}