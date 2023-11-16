using System;
using System.Collections.Generic;
using TMPro;

namespace ZCore
{
    public class UIDebug : CoreSingletonBehavior<UIDebug>
    {
        public TextMeshProUGUI ui;
        public List<Entry> entriesNew = new List<Entry>();

        public void Update()
        {
            Refresh();
        }

        public void OnDrawGizmos()
        {
            //UnityEditor.Handles.Label()
        }

        public void Register(string inName, Func<object> getLatestValue)
        {
            var entry = new Entry()
            {
                name = inName,
                value = getLatestValue
            };
            entriesNew.Add(entry);
        }

        public void Refresh()
        {
            var text = "";
            foreach (var entry in entriesNew)
            {
                text += $"{entry.name}: {entry.value()}\n";
            }

            ui.text = text;
        }

        public class Entry
        {
            public string name;
            public Func<object> value;
        }
    }
}