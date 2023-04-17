using System;
using Glaidiator.Model.Utils;

namespace Glaidiator.Model.Actions
{
    public class ActionInfo
    {
        public int ID { get; }
        public string Name { get; }
        public float Cost { get; }
        public bool CanMove { get; }
        public bool CanAction { get; }
        public Timer Duration { get; set; }

        public ActionInfo(int id, string name, float cost, bool canMove, bool canAction, float duration)
        {
            ID = id;
            Name = name;
            Cost = cost;
            CanMove = canMove;
            CanAction = canAction;
            Duration = new Timer(duration);
        }
    }
}