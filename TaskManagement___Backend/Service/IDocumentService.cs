using System.Reflection.Metadata;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service
{
    public interface IDocumentService
    {
        public Task<(bool, string,int)> SaveDocument(int userId, string filePath, string fileName);

        public Task<bool> DeleteDocument(int docId);
        public Task<IQueryable> GetDocumentByUserId(int userId);
        public Task<List<Documents>> GetDocument(int id);

    }
}
