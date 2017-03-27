using EFTest.Dto;

namespace EFTest
{
    public interface IUserAdder
    {
        void AddUser(IDtoUser userToAdd);
    }
}