using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface ICounterService
    {
        public Task<int> ProjectCounter();
        public Task<int> TaskCounter();
    }
}
