namespace EFTest.DbContext
{
    public interface IComment
    {
        string Body { get; set; }
        int Id { get; set; }
    }
}