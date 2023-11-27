using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : PassiveTaskObject
{
    [SerializeField] List<EntrySection> EntrySections;
    [SerializeField] List<ExitSection> ExitSections;
    [SerializeField] List<InsideSection> InsideSections;
    private Dictionary<Transform, List<Section>> SectionEntries = new Dictionary<Transform, List<Section>>();
    private List<Section> Sections;
    private List<Section> MainSections;
    private bool playerEnteredIntersection;

    private void Awake()
    {
        EntryAndExitSetup();
    }

    private void EntryAndExitSetup()
    {
        MainSections = new List<Section>();
        MainSections.AddRange(InsideSections);
        Sections = new List<Section>();
        Sections.AddRange(EntrySections);
        Sections.AddRange(ExitSections);
        Sections.AddRange(InsideSections);
        foreach (var s in Sections) {
            s.onFail += () => onFailState(this);
            s.onZoneEntry += CheckZoneEntry;
        }
        foreach (var entry in EntrySections) {
            entry.onEntry += CheckEntry;
            entry.checkAIYields += CheckYieldingAI;
            if (entry.mainRoad)
                MainSections.Add(entry);
        }
        foreach (var exit in ExitSections)
            exit.onSuccessfulPass += PassComplete;
    }

    private void CheckEntry(EntrySection entry)
    {
        if (playerEnteredIntersection)
            onFailState?.Invoke(this);
        Debug.Log($"Player entering intersection through {entry.gameObject.name}");
        entry.playerHasEntered = true;
        playerEnteredIntersection = true;
    }

    private void CheckZoneEntry(Section section, Transform t)
    {
        if (!SectionEntries.ContainsKey(t)) {
            SectionEntries.Add(t, new List<Section>() { section });
        }
        else if (section is ExitSection) {
            SectionEntries.Remove(t);
        } else if (section is InsideSection) {
            var inSection = section as InsideSection;
            SectionEntries[t].Add(section);
            if (SectionEntries[t].Count > 1) {
                InsideSection firstInsideSec = SectionEntries[t][1] as InsideSection;
                if (firstInsideSec == null)
                    onFailState.Invoke(this);
                else if (firstInsideSec == section)
                    inSection.SetCarRelativeToFirst(t, RelativeToFirst.Equal);
                else if (firstInsideSec.leftTurn == section)
                    inSection.SetCarRelativeToFirst(t, RelativeToFirst.LeftTurn);
                else if (firstInsideSec.straightSec == section) {
                    inSection.SetCarRelativeToFirst(t, RelativeToFirst.Straight);
                }
                else
                    inSection.SetCarRelativeToFirst(t, RelativeToFirst.Other);
            } else
                onFailState?.Invoke(this);
        }
    }

    private void CheckYieldingAI(AIDriver d, Transform t, EntrySection eSection)
    {
        bool stopCar = false;
        if (!eSection.mainRoad)
            foreach (var section in MainSections)
                if (section.CarsInZoneWithout(t) > 0)
                    stopCar = true;
        d.hasToStopCar = stopCar;
    }

    private void PassComplete() 
    {
        Debug.Log("Player passed intersection successfully");
        foreach (var s in Sections)
            s.ResetPlayerInfo();
        playerEnteredIntersection = false;
    }
}
