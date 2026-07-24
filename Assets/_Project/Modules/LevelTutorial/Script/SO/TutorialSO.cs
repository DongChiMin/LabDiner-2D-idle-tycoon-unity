using System.Collections.Generic;
using LabDiner.Shared.Event;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialSO", menuName = "SO/Tutorial/TutorialSO")]
public class TutorialSO : ScriptableObject
{
    [SerializeField] private string _tutorialID;
    // [SerializeField] List<UIPopupEvent> _keepOpenUIs = new List<UIPopupEvent>();
    [SerializeField] private bool _turnOffAllUI = true;

    public string ID => _tutorialID;
    public bool TurnOffAllUI => _turnOffAllUI;
}