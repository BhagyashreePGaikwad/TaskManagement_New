
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using TaskManagement_April_.Context;
using TaskManagement_April_.Model;

namespace TaskManagement_April_.Service.Implementation
{
    public class DocumentService : IDocumentService
    {
        #region Variable
        private TaskManagementContext _dbcontext;
        #endregion
        #region Constructor
        public DocumentService(TaskManagementContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        #endregion
        #region Method
        public Task<bool> DeleteDocument(int docId)
        {
            try
            {
                var doc = _dbcontext.Documents.Where(s =>s.DocId == docId).FirstOrDefault();
                if (doc != null)
                {
                    _dbcontext.Documents.Remove(doc);
                    _dbcontext.SaveChanges();
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);

            }
            catch (Exception ex) 
            {
                throw;
            }

        }

        public async Task<IQueryable> GetDocumentByUserId(int userId)
        {
            try
            {
                var doclist=await (from i in _dbcontext.Documents
                                   where userId==i.userId
                                   select new
                                   {
                                       i.userId,
                                       i.DocId,
                                       i.FileName,
                                       i.FilePath
                                   }).ToListAsync();
                return doclist.AsQueryable();

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<Documents>> GetDocument(int id)
        {
            try
            {
                var docList = await (from i in _dbcontext.Documents
                                     where i.DocId == id
                                     select new Documents 
                                     {
                                         userId = i.userId,
                                         DocId = i.DocId,
                                         FileName = i.FileName,
                                         FilePath = i.FilePath
                                     }).ToListAsync();
                return docList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public Task<(bool,string,int)> SaveDocument(int userId,  string filePath ,string fileName)
        {
            try
            {
                var doc=_dbcontext.Documents.Where(s=>fileName==s.FileName && s.userId==userId).FirstOrDefault();
                if (doc==null)
                {
                    var newDoc=new Documents(){
                        userId=userId,
                        FileName=fileName,
                        FilePath=filePath
                    };
                    _dbcontext.Documents.Add(newDoc);
                    _dbcontext.SaveChanges();
                    return Task.FromResult((true,"Document uploaded successfully",200));
                }
                else
                {
                    return Task.FromResult((false,"Document already exist",400));
                }
                return Task.FromResult((false, "Something went wrong", 400));
            }
            catch (Exception ex)
            {
                return Task.FromResult((false,ex.Message,400));
                throw;
            }

        }
        #endregion
    }
}
