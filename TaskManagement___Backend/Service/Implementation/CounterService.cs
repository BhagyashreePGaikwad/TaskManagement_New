using TaskManagement_April_.Context;

namespace TaskManagement_April_.Service.Implementation
{
    public class CounterService : ICounterService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        #endregion
        #region Construstor
        public CounterService(TaskManagementContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion
        public Task<int> ProjectCounter()
        {
            try
            {
                var count = _dbcontext.Counter.FirstOrDefault(s => s.Id == 1);
                var value = count.CountNum;
                count.CountNum = count.CountNum + 1;
                _dbcontext.SaveChanges();
                return Task.FromResult(value);
            }catch(Exception ex)
            {
                return Task.FromResult(0);
            }           

        }

        public Task<int> TaskCounter()
        {
            try
            {
                var count = _dbcontext.Counter.FirstOrDefault(s => s.Id == 2);
                var value = count.CountNum;
                count.CountNum = count.CountNum + 1;
                _dbcontext.SaveChanges();
                return Task.FromResult(value);
            }
            catch (Exception ex)
            {
                return Task.FromResult(0);
            }

        }
    }
}
