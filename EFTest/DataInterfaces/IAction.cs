using EFTest.DbContext;
using System;

namespace EFTest
{
    public interface IAction
    {
        string Body { get; set; }
        int Id { get; set; }
        User Initiator { get; set; }
        DateTime TimeStamp { get; set; }
        ActionType Type { get; set; }
    }
}