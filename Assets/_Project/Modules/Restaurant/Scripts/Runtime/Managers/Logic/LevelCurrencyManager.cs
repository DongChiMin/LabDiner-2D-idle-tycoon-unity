using LabDiner.Restaurant.Pooling;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Enum;
using LabDiner.Shared.Event;
using LabDiner.Shared.Extension;
using LabDiner.Shared.SO;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.Manager
{
    // public class LevelCurrencyManager : MonoBehaviour
    // {
    //     [Header("Data Source")]
    //     [SerializeField] private DoubleRuntimeSO _coinData;
    //     [SerializeField] private IntRuntimeSO _gemData;

    //     [Header("Coin Events")]
    //     [SerializeField] private LevelCoinFlyEvent _onCoinFlyAdded;

    //     [Header("Gem Events")]
    //     [SerializeField] private LevelGemEvent _onGemAdded;
    //     [SerializeField] private LevelGemFlyEvent _onGemFlyAdded;
    //     [SerializeField] private LevelGemEvent _onGemSpent;

    //     [Header("[DEBUG]")]

    //     [Tooltip("Có thể nhập đơn vị a, b, c để test nhanh số tiền lớn")]
    //     [SerializeField] private string _cheatCoin = "5.23a";
    //     [ReadOnly] [SerializeField] private double _currentCoin;
    //     [ReadOnly] [SerializeField] private int _currentGem;

    //     void OnEnable()
    //     {
    //         _onCoinFlyAdded.Register(HandleCoinFlyAdded);

    //         _onGemAdded.Register(AddGem);
    //         _onGemSpent.Register(SpendGem);
    //         _onGemFlyAdded.Register(HandleGemFlyAdded);
    //     }

    //     void OnDisable()
    //     {
    //         _onCoinFlyAdded.Unregister(HandleCoinFlyAdded);

    //         _onGemAdded.Unregister(AddGem);
    //         _onGemSpent.Unregister(SpendGem);
    //         _onGemFlyAdded.Unregister(HandleGemFlyAdded);
    //     }

    //     private void AddGem(int amount)
    //     {
    //         _gemData.Add(amount);
    //         _currentGem = _gemData.Value;
    //     }

    //     private void SpendGem(int amount)
    //     {
    //         _gemData.Add(-amount);
    //         _currentGem = _gemData.Value;
    //     }

    //     private void HandleGemFlyAdded(GemRewardData data)
    //     {
    //         _gemData.Add(data.RewardValue);
    //         _currentGem = _gemData.Value;
    //     }

    //     private void HandleCoinFlyAdded(CoinRewardData data)
    //     {
    //         _coinData.Add(data.RewardValue);
    //         _currentCoin = _coinData.Value;
    //     }

    //     void OnValidate()
    //     {
    //         if(!string.IsNullOrEmpty(_cheatCoin))
    //         {
    //             double parsedValue = CurrencyFormatter.Format(_cheatCoin);
    //             _coinData.SetValue(parsedValue);
    //             _currentCoin = parsedValue;
    //         }
    //     }
    // }
}