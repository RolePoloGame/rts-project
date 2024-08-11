using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using RTS.Core;
using DG.Tweening;
using System;

namespace RTS.Agents
{
    public class Pathfinder : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float reachedDistance = 0.5f;
        [field: SerializeField] public float MovementSpeed { get; set; } = 1.0f;
        [field: SerializeField] public float RotationSpeed { get; set; } = 1.0f;
        [field: SerializeField] public bool RandomMovement { get; set; } = false;

        private bool waitingForPathCalculation = false;
        private bool hasPath = false;
        private bool reachedDestination;
        private float speedFactor = 1.0f;

        private List<Vector3> currentPath;
        private Vector3 nextPos;

        private Tweener moveTween;
        private Tweener rotateTween;
        private bool movementStarts;
        private bool movementStops;

        public bool ReachedDestination => reachedDestination;

        public event Action OnArrvied;

        /// <summary>Updates the AI's destination every frame</summary>
        void Update()
        {
            if (hasPath)
            {
                HandleMovement();
                return;
            }

            if (waitingForPathCalculation) return;
            if (!RandomMovement) return;
            SearchForPath();
        }
        private void OnDestroy()
        {
            if (moveTween != null) moveTween.Kill();
            if (rotateTween != null) rotateTween.Kill();
        }
        private void HandleMovement()
        {
            if (!ReachedStep()) return;
            if (currentPath == null || currentPath.Count == 0)
            {
                OnArrvied?.Invoke();
                hasPath = false;
                return;
            }
            nextPos = currentPath.Dequeue();
            if (currentPath.Count == 0)
                movementStops = true;
            UpdateMoveTweeen();
            UpdateRotationTween();
            movementStarts = false;
            movementStops = false;
        }
        private void UpdateRotationTween()
        {
            var angle = Vector3.Angle(nextPos, transform.position);
            if (rotateTween != null) rotateTween.Kill();
            float duration = angle / RotationSpeed * speedFactor;
            Quaternion lookQuaternion = Quaternion.LookRotation((nextPos - transform.position).normalized, Vector3.up);
            rotateTween = transform.DORotateQuaternion(lookQuaternion, duration).SetEase(GetEase());
        }

        private void UpdateMoveTweeen()
        {
            var distance = Vector3.Distance(nextPos, transform.position);
            if (moveTween != null) moveTween.Kill();
            float angularDeceleration = GetAngularDecelerationRate();
            var speed = MovementSpeed * speedFactor;
            distance += distance * angularDeceleration / RotationSpeed;
            float duration = distance / speed;
            moveTween = transform.DOMove(nextPos, duration).SetEase(GetEase());
        }
        private float GetAngularDecelerationRate()
        {
            float angleToTarget = Vector3.Angle(nextPos, transform.position);
            if (angleToTarget < 0.0f)
                angleToTarget *= -1.0f;
            if (angleToTarget > 180.0f)
                angleToTarget = 360.0f - angleToTarget;
            return angleToTarget / 180.0f;
        }
        private Ease GetEase()
        {
            if (movementStarts)
                return Ease.InSine;
            if (movementStops)
                return Ease.Linear;
            return Ease.Linear;
        }

        private bool ReachedStep() => reachedDestination = Vector3.Distance(nextPos, transform.position) <= reachedDistance;

        private void SearchForPath()
        {
            movementStarts = false;
            movementStops = false;
            waitingForPathCalculation = true;
            Vector3 from = transform.position;
            Vector3 to = GetRandomPoint();
            var path = ABPath.Construct(from, to, OnPathComplete);
            AstarPath.StartPath(path);
        }

        private Vector3 GetRandomPoint()
        {
            var theta = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
            var radius = UnityEngine.Random.Range(15.0f, 25.0f);
            var x = radius * Mathf.Cos(theta);
            var z = radius * Mathf.Sin(theta);
            return new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        }

        private void OnPathComplete(Path newPath)
        {
            var p = newPath as ABPath;
            waitingForPathCalculation = false;

            // Increase the reference count on the new path.
            // This is used for object pooling to reduce allocations.
            p.Claim(this);

            // Path couldn't be calculated of some reason.
            // More info in p.errorLog (debug string)
            if (p.error || p.CompleteState != PathCompleteState.Complete)
            {
                p.Release(this);
                Debug.Log($"Path is not valid {p.error} {p.CompleteState}");
                SearchForPath();
                return;
            }

            currentPath = p.vectorPath;
            movementStarts = true;
            hasPath = true;
        }

        public void SetSpeedFactor(float value)
        {
            speedFactor = value;
            UpdateTweens();
        }

        private void UpdateTweens()
        {
            movementStarts = speedFactor > 0;

            if (moveTween != null)
            {
                if (speedFactor == 0.0f)
                    moveTween.Pause();
                else
                    UpdateMoveTweeen();
            }

            if (rotateTween != null)
            {
                if (speedFactor == 0.0f)
                    rotateTween.Pause();
                else
                    UpdateRotationTween();
            }
        }
        private void OnDrawGizmos()
        {
            if (!hasPath) return;
            Gizmos.DrawLine(transform.position, nextPos);
            Vector3 prev = transform.position;
            Vector3 next;
            Gizmos.color = Color.yellow;
            foreach (var it in currentPath)
            {
                next = it;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
            Gizmos.color = Color.cyan;
        }
    }
}
