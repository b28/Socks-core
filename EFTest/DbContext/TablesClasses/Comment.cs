namespace EFTest.DbContext
{
    public class Comment : IComment
    {
        public int Id { get; set; }
        public string Body { get; set; }
    }
}