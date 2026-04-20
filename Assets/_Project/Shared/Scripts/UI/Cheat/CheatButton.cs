using LabDiner.Shared.Event;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Shared
{
    public class CheatButton : MonoBehaviour
    {
        [Header("Gem Add")]
        [SerializeField] private Button _gemAddedButton;
        [SerializeField] private LevelGemEvent _onGemAdded;
        [SerializeField] private int _gemAddedValue;

        [Header("Gem Fly")]
        [SerializeField] private Button _gemFlyButton;
        [SerializeField] private LevelGemFlyEvent _onGemFlyAdded;
        [SerializeField] private Transform _gemFlyStartPos;
        [SerializeField] private int _gemFlyValue;

        [Header("Coin Add")]
        [SerializeField] private Button _coinAddedButton;
        [SerializeField] private LevelCoinEvent _onCoinAdded;
        [SerializeField] private int _coinAddedValue;

        [Header("Coin Fly")]
        [SerializeField] private Button _coinFlyButton;
        [SerializeField] private LevelCoinFlyEvent _onCoinFlyAdded;
        [SerializeField] private Transform _coinFlyStartPos;
        [SerializeField] private int _flyValue;


        void Awake()
        {
            _gemAddedButton.onClick.AddListener(() => _onGemAdded.Raise(_gemAddedValue));
            _gemFlyButton.onClick.AddListener(() => 
            {
                GemRewardData data = new GemRewardData
                {
                    startPos = _gemFlyStartPos.position,  
                    RewardValue = _gemFlyValue
                };
                _onGemFlyAdded.Raise(data);
            });

            _coinAddedButton.onClick.AddListener(() => _onCoinAdded.Raise(_coinAddedValue));
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
