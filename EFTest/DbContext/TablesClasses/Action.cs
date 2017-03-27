using System;

namespace EFTest.DbContext
{
    public class Action : IAction
    {
        public int Id { get; set; }
        public virtual User Initiator { get; set; }
        public string Body { get; set; }
        public ActionType Type { get; set; }
        public DateTime TimeStamp { get; set; }

    }

    public enum ActionType
    {
        GetBalance = 1,
        Dispensing,
        BoxInitialization,
        UnloadingSploit,
        Authentication,
        UserAdding,
        UserRemoving,
        UserModifying
    }
}
