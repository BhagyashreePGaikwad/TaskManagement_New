using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface IRoleService
    {
        public Task<IQueryable> GetRole();
        public Task<string> GetRoleNameById(int id);


    }
}
