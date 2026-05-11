
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LabDiner.Restaurant.Workflow
{
    public class StaffMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _stoppingDistance = 0.1f;
        [SerializeField] private float _speed = 3.5f;

        void Awake()
        {
            _agent.speed = _speed;
            _agent.stoppingDistance = _stoppingDistance;
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        // Hàm này giúp Coroutine bên ngoài có thể "đợi" cho đến khi đến đích
        public IEnumerator MoveTo(Vector3 destination)
        {
            _agent.SetDestination(destination);

            yield return null;

            // Đợi cho đến khi Agent bắt đầu tính toán xong đường đi
            yield return new WaitUntil(() => !_agent.pathPending);

            // Đợi cho đến khi khoảng cách còn lại nhỏ hơn stoppingDistance
            while (_agent.remainingDistance > _agent.stoppingDistance)
            {
                yield return null;
            }
        }

        public void SetZToZero()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        public void UpgradeMoveSpeed(float speedBuffValue)
        {
            _agent.speed += speedBuffValue;
        }
    }
}