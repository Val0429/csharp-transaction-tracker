
namespace Constant
{
    public enum ReadyState : ushort
    {
        New = 0,
        Ready = 1,
        Modify = 2,
        NotInUse = 3,
        Delete = 4,
        JustAdd = 5, // add permission to user, then change to New
        Saving = 6,
        Unavailable = 9,
        ReSync = 10,
    }

    public enum ManagerReadyState : ushort
    {
        New = 0,
        Ready = 4,
        Saving = 5,
        Loading = 6,
        MajorModify = 8,
        Unavailable = 9,
    }
}