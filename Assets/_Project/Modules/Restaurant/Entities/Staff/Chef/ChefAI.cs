
using System.Collections;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class ChefAI : MonoBehaviour
    {
        [Header("Event")]
        [SerializeField] private ChefEvent _onChefAvailable;
        [Header("[DEBUG]")]
        [SerializeField] private CookingTask currentTask;

        private StaffMover _mover;
        private ChefBehavior _behavior;
        private ChefContext _context;

        void Start()
        {
            _mover = GetComponent<StaffMover>();
            _behavior = GetComponent<ChefBehavior>();
            _context = GetComponent<ChefContext>();
        }

        public IEnumerator DoTask(CookingTask task)
        {
            currentTask = task;
            Station station = task.StationTarget;
            PassTable passTable = task.PassTableTarget;


            //1. Di chuyển đến vị trí của Station
            yield return _mover.MoveTo(station.WorkPos.position);

            //2. Nấu
            yield return _behavior.Cook(task);

            //3. Di chuyển đến vị trí PassTable (nếu có)
            yield return _mover.MoveTo(passTable.WorkPos.position);

            //4. Đặt món lên PassTable
            yield return _behavior.PlaceOnPassTable(task);

            //5. Quay về vị trí nghỉ ngơi hoặc làm task khác nếu có
            yield return Rest();

        }

        public void StartTask(CookingTask task)
        {
            StopAllCoroutines();
            StartCoroutine(DoTask(task));
        }

        IEnumerator Rest()
        {
            currentTask = null;
            _onChefAvailable.Raise(_context);
            yield return _mover.MoveTo(_context.RestPosition.position);
        }

        void LateUpdate() => _mover.SetZToZero();
    }
}