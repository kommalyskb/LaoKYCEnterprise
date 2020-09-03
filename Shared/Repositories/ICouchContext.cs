using MyCouch.Responses;
using Shared.Configs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Repositories
{
    public interface ICouchContext
    {
        Task<DocumentHeaderResponse> DeleteAsync(CouchDBHelper couchDBHelper, string id, string rev);
        Task<DocumentHeaderResponse> DeleteFileAsync(CouchDBHelper couchDBHelper, string id, string rev, string filename);
        Task<EntityResponse<T>> EditAsync<T>(CouchDBHelper couchDBHelper, T model) where T : class;
        Task<string> EditFileAsync(CouchDBHelper couchDBHelper, string id, string rev, List<AttachmentRequest> files);
        Task<GetEntityResponse<T>> GetAsync<T>(CouchDBHelper couchDBHelper, string id, string rev = null) where T : class;
        Task<AttachmentResponse> GetFileAsync(CouchDBHelper couchDBHelper, string id, string filename, string rev = null);
        Task<EntityResponse<T>> InsertAsync<T>(CouchDBHelper couchDBHelper, T model) where T : class;
        Task<EntityResponse<T>> InsertWithFileAsync<T>(CouchDBHelper couchDBHelper, T model, List<AttachmentRequest> files) where T : class;
        Task<ViewQueryResponse<T>> ViewQueryAsync<TRequest, T>(CouchDBHelper couchDBHelper, string designName, string viewName, TRequest keys, int limit, int page, bool reduce, bool desc) where T : class where TRequest : class;
        Task<ViewQueryResponse<T>> ViewQueryAsync<T>(CouchDBHelper couchDBHelper, string designName, string viewName, string key, int limit, int page, bool reduce, bool desc) where T : class;
    }
    public class AttachmentRequest
    {
        public string FileName { get; set; }
        public string Ext { get; set; }
        public byte[] Contents { get; set; }

    }
}
