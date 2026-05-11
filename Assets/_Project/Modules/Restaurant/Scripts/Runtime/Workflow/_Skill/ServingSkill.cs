using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.UI;
using UnityEngine;

namespace LabDiner.Restaurant.Workflow
{
    public partial class ServingSkill : StaffSkill
    {
        [Header("References")]
        [SerializeField] private TaskRuntimeSO _taskRuntimeSO;
        
        [Header("Serve Settings")]
        [SerializeField] private float _serveDuration = 3f; // Thời gian để phục vụ một khách hàng
        [SerializeField] private StaffMover _mover;
        [SerializeField] private StaffProgressPieUI _progressPieUI;

        private Order _order;

        #region API
        public override TaskType SkillType => TaskType.Serving;

        #endregion

        public override IEnumerator Execute(BaseTask task, Action onComplete)
        {
            if (task is ServingTask servingTask)
            {
                //0. Chuẩn bị dữ liệu
                _order = servingTask.Order;
                GuestContext guest = _order.OrderBy;
                Debug_FetchData(servingTask);

                //1. Di chuyển đến vị trí phục vụ
                yield return _mover.MoveTo(servingTask.Location.position);

                //2. Thực hiện phục vụ
                _progressPieUI.StartProgressPie(_serveDuration);
                yield return new WaitForSeconds(_serveDuration);

                //3. Cập nhật trạng thái đã phục vụ cho khách
                guest.SetServedStatus(true);

                //4. Tạo các CookingTask tương ứng cho từng món ăn trong order
                CreateCookingTask();
                onComplete?.Invoke();

                //5. Di chuyển về vị trí nghỉ ngơi
                yield return _mover.MoveTo(_staff.RestPosition.position);
            }
            else
            {
                Debug.LogError("Task không hợp lệ cho ServingSkill!");
            }
        }

        #region Private Methods
        private void CreateCookingTask() 
        {
            //Khi khách hàng được serve, phân rã thành các cookingTask mới
            foreach (var item in _order.OrderDict)
            {
                CoreStation station = item.Key;
                int quantity = item.Value;

                // 2. Với mỗi loại, lặp lại 'quantity' lần để tạo Task lẻ
                for (int i = 0; i < quantity; i++)
                {
                    CookingTask singleTask = new CookingTask(null, _order, station);
                    // 3. Đẩy vào hàng đợi chung của bếp
                    _taskRuntimeSO.Add(singleTask);
                }
            }
        }
        #endregion

        private partial void Debug_FetchData(ServingTask servingTask);
    }
}