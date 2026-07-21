using UnityEngine;

[CreateAssetMenu(fileName = "TutorialSO", menuName = "SO/Tutorial/TutorialSO")]
public class TutorialSO : ScriptableObject
{
    [SerializeField] private string _tutorialID;

    public string ID => _tutorialID;
}