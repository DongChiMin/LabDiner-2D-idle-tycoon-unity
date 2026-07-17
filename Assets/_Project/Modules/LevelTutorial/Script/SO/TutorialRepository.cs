using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialRepository", menuName = "SO/Tutorial/TutorialRepository")]
public class TutorialRepository : ScriptableObject
{
    public List<TutorialSO> Tutorials => _tutorials;

    [SerializeField] private List<TutorialSO> _tutorials;
}