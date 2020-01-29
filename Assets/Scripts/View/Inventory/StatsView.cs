using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsView : MonoBehaviour
{
    public Character.Stats BaseStat;
    public Text text;

    private IEnumerable<KeyValuePair<Character.Stats, int>> stats;

    void Start()
    {
        var toggleName = gameObject.GetComponentInChildren<Toggle>().GetComponentInChildren<Text>();
        toggleName.text = BaseStat.ToString();
        
        text.gameObject.SetActive(false);

        Events.EventManager.StartListening(Events.GameEventTypes.StatsRecalculated, StatsRecalculated);
    }

    public void ExpandCollapse(bool expand)
    {
        UpdateStats();
        text.gameObject.SetActive(expand);
    }

    private void UpdateStats()
    {
        var controller = GameObject.Find("PaladinGO").GetComponent<Controllers.CharController>();
        stats = controller.GetAllStatsInGroup(BaseStat);

        text.text = "";

        foreach(var s in stats)
            text.text += $"{s.Key, 30}   {s.Value}\n";
    }

    private void StatsRecalculated(EventArgs e)
    {
        UpdateStats();
    }
}
