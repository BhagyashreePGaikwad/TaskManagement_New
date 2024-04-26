using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface IUserService
    {
        public Task<IQueryable> GetUsers();
        public Task<IQueryable> GetUserById(int id);
        public Task<(bool,string,int)> SaveUser(User model);
        public Task<(bool,string,int)> UpdateUser(User model,int id);
        public Task<bool> DelUser(int id);
        public Task<bool> CheckValidUser(Login model);
        public Task<User> LoginUser(Login model);


    }
}
