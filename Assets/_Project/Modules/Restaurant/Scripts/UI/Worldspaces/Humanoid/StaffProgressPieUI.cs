using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class StaffProgressPieUI : MonoBehaviour
    {
        [SerializeField] private GameObject _parentVisual;
        [SerializeField] private Image _progressPie;

        void OnEnable()
        {
            ToggleProgressPie(false);
        }

        #region API
        public void StartProgressPie(float duration)
        {
            if(duration <= 0f) return;
            _progressPie.fillAmount = 0f;
            ToggleProgressPie(true);
            StartCoroutine(FillProgressPie(duration));
        }
        #endregion

        private void ToggleProgressPie(bool isOn)
        {
            _parentVisual.SetActive(isOn);
        }

        private IEnumerator FillProgressPie(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _progressPie.fillAmount = Mathf.Clamp01(elapsed / duration);
                yield return null;
            }
            ToggleProgressPie(false);
        }
    }
}
