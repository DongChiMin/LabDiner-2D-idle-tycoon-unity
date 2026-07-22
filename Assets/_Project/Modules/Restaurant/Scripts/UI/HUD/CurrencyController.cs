using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using LabDiner.Shared.SO;
using UnityEngine;

namespace LabDiner.Restaurant.UI
{
    public class CurrencyController : MonoBehaviour
    {
        
        [Header("Data")]
        [SerializeField] private DoubleRuntimeSO _coinRuntimeData;
        [SerializeField] private IntRuntimeSO _gemRuntimeData;
        [SerializeField] private ProgressSaveRuntimeSO _progressRuntimeSO;

        [Header("Events")]
        [SerializeField] private LevelConfigEvent _onLevelInit;
        [SerializeField] private LevelCoinFlyEvent _onCoinFlyAdded;
        [SerializeField] private LevelGemFlyEvent _onGemFlyAdded;

        [Header("References")]
        [SerializeField] private CurrencyHUD _currencyHUD;

        void OnEnable()
        {
            // _onLevelInit.Register(Init);

            _coinRuntimeData.OnValueChanged += UpdateCoinProgress;
            _onCoinFlyAdded.Register(HandleCoinFlyAdded);

            _gemRuntimeData.OnValueChanged += UpdateGemProgress;
            _onGemFlyAdded.Register(HandleGemFlyAdded);

            //Progress
            _progressRuntimeSO.OnProgressInject += LoadProgress;
        }

        void OnDisable()
        {
            // _onLevelInit.Unregister(Init);

            _coinRuntimeData.OnValueChanged -= UpdateCoinProgress;
            _onCoinFlyAdded.Unregister(HandleCoinFlyAdded);

            _gemRuntimeData.OnValueChanged -= UpdateGemProgress;
            _onGemFlyAdded.Unregister(HandleGemFlyAdded);

            //Progress
            _progressRuntimeSO.OnProgressInject -= LoadProgress;
        }

        public void LoadProgress(ProgressSaveRuntimeSO progressRuntimeSO)
        {
            double levelCoin = progressRuntimeSO.LevelProgressSave.levelCoin;
            int levelGem = progressRuntimeSO.PlayerSave.Gem;
            _coinRuntimeData.SetValue(levelCoin);
            _gemRuntimeData.SetValue(levelGem);
        }

        public void UpdateCoinProgress(double newCoinAmount)
        {
            _progressRuntimeSO.LevelProgressSave.UpdateLevelCoin(newCoinAmount);
            _currencyHUD.RequestSetCoinText(newCoinAmount);
        }

        public void UpdateGemProgress(int newGemAmount)
        {
            _progressRuntimeSO.PlayerSave.UpdateGem(newGemAmount);
            _currencyHUD.RequestSetGemText(newGemAmount);
        }

        private void HandleCoinFlyAdded(CoinRewardData data)
        {
            _currencyHUD.PlayCoinFlyAnimation(data);
            _coinRuntimeData.SetValue(_coinRuntimeData.Value + data.RewardValue);
        }

        private void HandleGemFlyAdded(GemRewardData data)
        {
            _currencyHUD.PlayGemFlyAnimation(data);
            _coinRuntimeData.SetValue(_coinRuntimeData.Value + data.RewardValue);
        }
    }
}