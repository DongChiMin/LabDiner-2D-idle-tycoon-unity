using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Runtime
{
    public class GuestManager : MonoBehaviour, ILevelInitializable
    {
        [Header("References")]
        [SerializeField] private DiningSeatRuntimeSetSO _diningSeatRuntimeSet;
        [Header("Policy (Optional)")]
        [SerializeField] private MonoBehaviour OrderComposerBehaviour;
        IOrderComposer OrderComposer => OrderComposerBehaviour as IOrderComposer;
        private DefaultOrderComposer _defaultOrderComposer = new DefaultOrderComposer();
        private bool _hasWaitingLine;

        #region API
        public void Init(LevelConfigSO config)
        {
            _hasWaitingLine = config.WaitingLine;
        }

        public void Assign(GuestContext guest, Vector3 exitPos, int maxUniqueStations, int maxTotalQty)
        {
            // 1) Tạo order cho guest (linh hoạt các kiểu order khác nhau)
            Order order = OrderComposer != null
                ? OrderComposer.Compose(guest, maxUniqueStations, maxTotalQty)
                : _defaultOrderComposer.Compose(guest, maxUniqueStations, maxTotalQty);

            // 2) Lấy thông tin tình trạng nhà hàng hiện tại
            List<DiningSeat> availableSeats = _diningSeatRuntimeSet.GetAvailableSeats();

            // 3) Routing giữ nguyên thứ tự ưu tiên như GuestManager hiện tại:
            //    - nếu waiting line có người -> guest mới join line
            if (_hasWaitingLine && LevelManagerContext.Instance.WaitingLineManager.HasWaitingGuest)
            {
                Vector3 destination = LevelManagerContext.Instance.WaitingLineManager.AddToWaitingLine(guest);
                guest.Setup(order, destination, exitPos);
                return;
            }

            //    - nếu có seat -> occupy + đi vào seat
            if (availableSeats.Count > 0)
            {
                DiningSeat selectedSeat = availableSeats[Random.Range(0, availableSeats.Count)];
                _diningSeatRuntimeSet.SetOccupy(selectedSeat, guest);
                guest.Setup(order, selectedSeat.transform.position, exitPos, selectedSeat);
                return;
            }

            //    - nếu không có seat nhưng có waiting line -> join line
            if (_hasWaitingLine)
            {
                Vector3 destination = LevelManagerContext.Instance.WaitingLineManager.AddToWaitingLine(guest);
                guest.Setup(order, destination, exitPos);
                return;
            }

            // 4) fallback: nếu không có seat và không có waiting line -> loại bỏ guest
            LevelManagerContext.Instance.GuestSpawner.RemoveGuest(guest);
        }
        #endregion
    }
}