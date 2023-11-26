using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : PassiveTaskObject
{
    [SerializeField] List<EntrySection> EntrySections;
    [SerializeField] List<ExitSection> ExitSections;
    private List<Section> Sections;
    private bool playerEnteredIntersection;

    private void Awake()
    {
        EntryAndExitSetup();
    }

    private void EntryAndExitSetup()
    {
        Sections = new List<Section>();
        Sections.AddRange(EntrySections);
        Sections.AddRange(ExitSections);
        foreach (var s in Sections)
            s.onFail += () => onFailState(this);
        foreach (var entry in EntrySections)
            entry.onEntry += CheckEntry;
        foreach (var exit in ExitSections)
            exit.onSuccessfulPass += PassComplete;
    }

    private void CheckEntry(EntrySection entry)
    {
        if (playerEnteredIntersection)
            onFailState?.Invoke(this);
        Debug.Log("Player entering intersection");
        entry.playerHasEntered = true;
        playerEnteredIntersection = true;
    }

    private void PassComplete() 
    {
        Debug.Log("Player passed intersection successfully");
        foreach (var s in Sections)
            s.ResetPlayerInfo();
        playerEnteredIntersection = false;
    }
}
