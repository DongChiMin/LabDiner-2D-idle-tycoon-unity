using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.UI;
using UnityEngine;

namespace LabDiner.Restaurant.Workflow
{
    public partial class CookingSkill : StaffSkill
    {
        [Header("Cook Settings")]
        [SerializeField] private StaffMover _mover;
        [SerializeField] private float cookMultiplier = 1f;
        [SerializeField] private float _giveFoodDuration = 0f;
        [SerializeField] private float placeOnPassTableDuration = 0f;
        [SerializeField] private StaffProgressPieUI _progressPieUI;
        [SerializeField] private StaffCarryDishUI _carryDishUI;
        [SerializeField] private PassTableRuntimeSO _passTableRuntimeSO;

        public override TaskType SkillType => TaskType.Cooking;
        public override IEnumerator Execute(BaseTask task, Action onComplete)
        {
            if(task is CookingTask cookingTask)
            {
                Debug_FetchData(cookingTask);

                //0. Chuẩn bị các thông số cần thiết
                Station availableStation = cookingTask.CoreStation.PopAvailableStation();
                PassTable passTable = _passTableRuntimeSO.GetAvailablePassTable();
                CoreStation coreStation = cookingTask.CoreStation;
                GuestContext guest = cookingTask.Order.OrderBy;
                cookingTask.SetStationTarget(availableStation);
                cookingTask.SetPassTableTarget(passTable);
                float cookTime = coreStation.RawProcessTime / (1 + cookMultiplier);
                Vector3 guestPos = cookingTask.Order.OrderBy.DiningSeat.WorkPos.position;

                //1. Di chuyển đến vị trí nấu ăn
                yield return _mover.MoveTo(availableStation.WorkPos.position);

                //2. Nấu ăn
                _progressPieUI.StartProgressPie(cookTime);
                yield return new WaitForSeconds(cookTime);
                cookingTask.StationTarget.SetStatus(true);
                cookingTask.SetProfit(coreStation.CurrentProfit);
                _carryDishUI.UpdateCookingTaskPrice(cookingTask);
                _carryDishUI.CarryDish(cookingTask);

                //3.1 Nếu có PassTable
                if(passTable != null)
                {
                    //3.1.1 Di chuyển đến PassTable và đặt món lên pasTable
                    yield return _mover.MoveTo(passTable.WorkPos_PutOn.position);

                    //3.1.2 Đặt món lên PassTable
                    cookingTask.PassTableTarget.PlaceTaskOnPassTable(cookingTask);
                    // Di chuyển món ăn đến PassTable, bật hiệu ứng đặt món...
                    _progressPieUI.StartProgressPie(placeOnPassTableDuration);
                    yield return new WaitForSeconds(placeOnPassTableDuration);
                    _carryDishUI.Finish(cookingTask);
                }
                //3.2. Nếu không có PassTable thì đi thẳng đến khách
                else
                {
                    //3.2.1. Di chuyển đến vị trí khách
                    yield return _mover.MoveTo(guestPos);

                    //3.2.2. Đưa món cho khách
                    guest.ReceiveFood(cookingTask);
                    yield return new WaitForSeconds(_giveFoodDuration);
                    _carryDishUI.Finish(cookingTask);
                }

                //4. Hoàn thành
                Debug_FetchData(null);
                onComplete?.Invoke();

                //5. Di chuyển về vị trí nghỉ ngơi
                yield return _mover.MoveTo(_staff.RestPosition.position);
            }
            else
            {
                Debug.LogError("Task không hợp lệ cho CookingSkill!");
            }
        }

        private partial void Debug_FetchData(CookingTask cookingTask);
    }
}