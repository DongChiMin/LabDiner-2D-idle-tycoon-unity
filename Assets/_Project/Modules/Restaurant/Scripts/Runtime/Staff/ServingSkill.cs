using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.UI;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public partial class ServingSkill : StaffSkill
    {
        [SerializeField] private float _serveDuration = 3f; // Thời gian để phục vụ một khách hàng
        [SerializeField] private StaffMover _mover;
        [SerializeField] private StaffProgressPieUI _progressPieUI;

        #region API
        public override TaskType SkillType => TaskType.Serving;

        public override IEnumerator PerformTask(BaseTask task, Action onComplete)
            {
                if(task is ServingTask servingTask)
                {
                    Debug_FetchData(servingTask);

                    //1. Di chuyển đến vị trí phục vụ
                    yield return _mover.MoveTo(servingTask.Location.position);

                    //2. Thực hiện phục vụ
                    _progressPieUI.StartProgressPie(_serveDuration);
                    yield return new WaitForSeconds(_serveDuration);

                    //3. Cập nhật trạng thái đã phục vụ cho khách
                    servingTask.Guest.SetServedStatus(true);

                    //4. Gọi callback hoàn thành
                    onComplete?.Invoke();
                    
                    //5. Di chuyển về vị trí nghỉ ngơi
                    yield return _mover.MoveTo(_staff.RestPosition.position);
                }
                else
                {
                    Debug.LogError("Task không hợp lệ cho ServingSkill!");
                }
            }
        #endregion

        private partial void Debug_FetchData(ServingTask servingTask);
    }
}