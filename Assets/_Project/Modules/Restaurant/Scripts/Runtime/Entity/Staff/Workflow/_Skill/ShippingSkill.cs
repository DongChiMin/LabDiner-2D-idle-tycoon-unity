using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.UI;
using LabDiner.Shared.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Workflow
{
    public partial class ShippingSkill : StaffSkill
    {
        [Header("References")]
        [SerializeField] private TaskRuntimeSO _taskRuntimeSO;
        [SerializeField] private DoubleRuntimeSO _coinData;
        
        [Header("Shipping Settings")]
        [SerializeField] private StaffMover _mover;
        [SerializeField] private StaffCarryDishUI _carryDishUI;
        [SerializeField] private float _giveFoodDuration = 0f;

        private Order _order;

        #region API
        public override TaskType SkillType => TaskType.Shipping;

        #endregion

        public override IEnumerator Execute(BaseTask task, Action onComplete)
        {
            if (task is ShippingTask shippingTask)
            {
                //1. Di chuyển đến vị trí passTable để lấy đồ ăn
                yield return _mover.MoveTo(shippingTask.Location.position);

                //2. Lấy đồ ăn
                PassTable passTable = shippingTask.PassTable;
                CookingTask cookingTask = shippingTask.CookingTask;
                passTable.PickUpDish(cookingTask);
                _carryDishUI.UpdateCookingTaskPrice(cookingTask);
                _carryDishUI.CarryDish(cookingTask);


                //3.1 Di chuyển đến vị trí khách hàng
                GuestContext guest = cookingTask.Order.OrderBy;
                Vector3 guestPos = guest.DiningSeat.WorkPos.position;
                
                yield return _mover.MoveTo(guestPos);

                    //3.2. Đưa món cho khách
                    guest.ReceiveFood(cookingTask);
                    yield return new WaitForSeconds(_giveFoodDuration);
                    _carryDishUI.Finish(cookingTask);

                //4. Hoàn thành
                Debug_FetchData(null);
                _coinData.Add(cookingTask.Profit);
                onComplete?.Invoke();

                //5. Di chuyển về vị trí nghỉ ngơi
                yield return _mover.MoveTo(_staff.RestPosition.position);
            }
            else
            {
                Debug.LogError("Task không hợp lệ cho ShippingSkill!");
            }
        }

        partial void Debug_FetchData(ShippingTask shippingTask);
    }
}