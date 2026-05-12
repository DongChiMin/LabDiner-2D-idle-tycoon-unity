using LabDiner.Shared.Event;
using LabDiner.Shared.SO;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Shared
{
    public class CheatButton : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private DoubleRuntimeSO _coinData;
        [SerializeField] private IntRuntimeSO _gemData;

        [Header("Gem Add")]
        [SerializeField] private Button _gemAddedButton;
        [SerializeField] private int _gemAddedValue;

        [Header("Gem Fly")]
        [SerializeField] private Button _gemFlyButton;
        [SerializeField] private LevelGemFlyEvent _onGemFlyAdded;
        [SerializeField] private Transform _gemFlyStartPos;
        [SerializeField] private int _gemFlyValue;

        [Header("Coin Add")]
        [SerializeField] private Button _coinAddedButton;
        [SerializeField] private int _coinAddedValue;

        [Header("Coin Fly")]
        [SerializeField] private Button _coinFlyButton;
        [SerializeField] private LevelCoinFlyEvent _onCoinFlyAdded;
        [SerializeField] private Transform _coinFlyStartPos;
        [SerializeField] private int _flyValue;


        void Awake()
        {
            _gemAddedButton.onClick.AddListener(() => _gemData.Add(_gemAddedValue));
            _gemFlyButton.onClick.AddListener(() => 
            {
                GemRewardData data = new GemRewardData
                {
                    startPos = _gemFlyStartPos.position,  
                    RewardValue = _gemFlyValue
                };
                _onGemFlyAdded.Raise(data);
            });

            _coinAddedButton.onClick.AddListener(() => _coinData.Add(_coinAddedValue));
            _coinFlyButton.onClick.AddListener(() =>
            {
               CoinRewardData data = new CoinRewardData
               {
                   startPos = _coinFlyStartPos.position,
                   RewardValue = _flyValue
               };
                _onCoinFlyAdded.Raise(data); 
            });
        }
    }
}
