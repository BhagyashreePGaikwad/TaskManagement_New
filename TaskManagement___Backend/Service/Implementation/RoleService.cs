
using Microsoft.EntityFrameworkCore;
using TaskManagement_April_.Context;

namespace TaskManagement_April_.Service.Implementation
{
    public class RoleService : IRoleService
    {

        #region Variable
        private TaskManagementContext _dbcontext;
        #endregion

        #region Constructor
        public RoleService(TaskManagementContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion
        public async Task<IQueryable> GetRole()
        {
            try
            {
                var result = await (from i in _dbcontext.Role
                                   select new
                                   {
                                       i.Id,
                                       i.RoleName
                                   }).ToListAsync();
                return result.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> GetRoleNameById(int id)
        {
            try
            {
                var result = await _dbcontext.Role
                    .Where(e => e.Id == id)
                    .Select(x => x.RoleName)
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}
