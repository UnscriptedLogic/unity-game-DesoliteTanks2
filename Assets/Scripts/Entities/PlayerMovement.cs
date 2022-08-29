using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class PlayerMovement : MonoBehaviour, IMoveable
    {
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody rb;

        private Vector3 moveDirection;

        private void Update()
        {
            GetInput();
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            rb.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);

            if (moveDirection.magnitude > 0)
            {
                transform.forward = moveDirection;
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
    }
}