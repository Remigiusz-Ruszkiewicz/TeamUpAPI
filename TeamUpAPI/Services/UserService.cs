using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TeamUpAPI.Contracts.Requests;
using TeamUpAPI.Data;
using TeamUpAPI.Helpers;
using TeamUpAPI.Models;

namespace TeamUpAPI.Services
{
    public class UserService : IUserService
    {
        public UserService(DataContext dbcontext)
        {
            Dbcontext = dbcontext;
        }
        public DataContext Dbcontext { get; }
        User IUserService.AddUser(AddUserRequest userRequest)
        {
            try
            {
                Dbcontext.Database.BeginTransaction();
                User user = new(
                    )
                { Id = Guid.NewGuid(), Username = userRequest.Username, StartHour = userRequest.StartHour, EndHour = userRequest.EndHour, Age = userRequest.Age, Email = userRequest.Email, Password = userRequest.Password, FriendsList = userRequest.FriendsList, GamesList = userRequest.GamesList };
                Dbcontext.Set<User>().Add(user);
                Dbcontext.SaveChanges();


                Dbcontext.Database.CommitTransaction();
                return user;
            }
            catch (Exception)
            {
                Dbcontext.Database.RollbackTransaction();
                throw;
            }
        }

        Enums.OperationResult IUserService.DeleteUser(Guid id)
        {
            try
            {
                var user = Dbcontext.Set<User>()
                    .FirstOrDefault(x => x.Id == id);

                if (user == null)
                {
                    return Enums.OperationResult.NotFound;
                }

                Dbcontext.Database.BeginTransaction();

                Dbcontext.Set<User>().Remove(user);

                Dbcontext.SaveChanges();
                Dbcontext.Database.CommitTransaction();
                return Enums.OperationResult.Ok;
            }
            catch (Exception)
            {
                if (Dbcontext.Database != null)
                {
                    Dbcontext.Database.RollbackTransaction();
                }
                return Enums.OperationResult.Error;
            }
        }

        async Task<Enums.OperationResult> IUserService.EditUser(User user)
        {
            Dbcontext.Users.Update(user);
            await Dbcontext.SaveChangesAsync();
            return Enums.OperationResult.Ok;
        }

        Task<User> IUserService.GetUserByIdAsync(Guid id)
        {
            return Dbcontext.Users.SingleOrDefaultAsync((x) => x.Id == id);
        }

        Task<ICollection<User>> IUserService.GetUserFriendsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        async Task<ICollection<User>> IUserService.GetUsersAsync()
        {
            return await Dbcontext.Users.ToListAsync();

        }
    }
}
