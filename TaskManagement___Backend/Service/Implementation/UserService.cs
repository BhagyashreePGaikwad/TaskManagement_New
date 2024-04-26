using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service.Implementation
{
    public class UserService : IUserService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        #endregion

        #region Constructor
        public UserService(TaskManagementContext dbcontext) { 
            _dbcontext = dbcontext;
        }
        #endregion
        public Task<bool> DelUser(int id)
        {
            try
            {
                var user = _dbcontext.User.FirstOrDefault(x => x.Id == id);
                if (user != null)
                {
                    _dbcontext.User.Remove(user);
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<IQueryable> GetUserById(int id)
        {
            try
            {
                var result = await _dbcontext.User.Where(e => e.Id == id).Select(x =>
           new
           {
               x.Id,
               x.Name,
               x.Email
           }).ToListAsync();
                return result.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IQueryable> GetUsers()
        {
            try
            {
                var result = await(from i in _dbcontext.User
                                   select new
                                   {
                                       i.Id,
                                       i.Name,
                                       i.Email
                                   }).ToListAsync();
                return result.AsQueryable();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<(bool,string,int)> SaveUser(User model)
        {
            try
            {
                var user = _dbcontext.User.FirstOrDefault(s => s.Email == model.Email);
                if (user == null)
                {
                    _dbcontext.Add(model);
                    _dbcontext.SaveChanges();
                    return Task.FromResult((true, "User created successfully",200));
                }
                else
                {
                    return Task.FromResult((false, "Email Id already in use",400));
                }
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                if (innerException is SqlException sqlException && sqlException.Number == 547)
                {
                    return Task.FromResult((false, "Invalid RoleId", 400));
                }
                else
                {
                    return Task.FromResult((false, "Database error", 400));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult((false, ex.Message,400));
            }
        }


        public Task<(bool,string,int)> UpdateUser(User model,int id)
        {
            try
            {
                var user = _dbcontext.User.FirstOrDefault(s => s.Id == id);
                var Existemail=_dbcontext.User.FirstOrDefault(s=>s.Email == model.Email && s.Id!=id);
                if (user != null && Existemail==null)
                {
                   user.Name= model.Name;
                    user.Email=model.Email;
                    user.Password=model.Password;
                    user.RoleId=model.RoleId;
                    _dbcontext.SaveChanges();
                    return Task.FromResult((true,"User updated successfully",200));
                }
                else if (Existemail != null)
                {
                    return Task.FromResult((false, "Email Id already exist", 400));
                }
                else
                {
                    return Task.FromResult((false, "User does not exist",404));
                }
                return Task.FromResult((false, "Email Id already exist", 400));
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                if (innerException is SqlException sqlException && sqlException.Number == 547)
                {
                    return Task.FromResult((false, "Invalid RoleId",547));
                }
                else
                {
                    return Task.FromResult((false, "Database error", 400));
                }
            }
            catch (Exception ex)
            {

                return Task.FromResult((false, ex.Message, 400));
            }
        }

        public Task<bool> CheckValidUser(Login model)
        {
            try
            {

                var user = _dbcontext.User.FirstOrDefault(x => x.Email == model.Email);
                if (user != null)
                {
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }

        public async Task<User> LoginUser(Login model)
        {
            try
            {

                var user =await _dbcontext.User.FirstOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);
                if (user != null)
                {
                    return user;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
     

    }
}
