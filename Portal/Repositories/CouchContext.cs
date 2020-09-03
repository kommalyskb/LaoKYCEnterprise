using MyCouch;
using MyCouch.Requests;
using MyCouch.Responses;
using Shared.Configs;
using Shared.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Repositories
{
    public class CouchContext : ICouchContext
    {
        public async Task<DocumentHeaderResponse> DeleteAsync(CouchDBHelper couchDBHelper, string id, string rev)
        {
            DocumentHeaderResponse result;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {
                //Using anonymous entities
                var req = new DeleteDocumentRequest(id, rev);
                result = await client.Documents.DeleteAsync(req);

            }

            return result;
        }

        public async Task<DocumentHeaderResponse> DeleteFileAsync(CouchDBHelper couchDBHelper, string id, string rev, string filename)
        {
            DocumentHeaderResponse result;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {
                //Using anonymous entities
                result = await client.Attachments.DeleteAsync(id, rev, filename);

            }

            return result;
        }

        public async Task<EntityResponse<T>> EditAsync<T>(CouchDBHelper couchDBHelper, T model) where T : class
        {
            EntityResponse<T> result;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {
                //Using anonymous entities
                result = await client.Entities.PutAsync(model);

            }

            return result;
        }

        public async Task<string> EditFileAsync(CouchDBHelper couchDBHelper, string id, string rev, List<AttachmentRequest> files)
        {
            int i = 0;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {
                //Using anonymous entities
                foreach (var f in files)
                {
                    string ctype = MimeMapping.MimeUtility.GetMimeMapping(f.FileName); // get content type
                    var req = new PutAttachmentRequest(id, rev, f.FileName, ctype, f.Contents);
                    await client.Attachments.PutAsync(request: req);
                    i++;
                }



            }
            string result = string.Format($"{0} update file completed", i);
            return await Task.FromResult(result);
        }

        public async Task<GetEntityResponse<T>> GetAsync<T>(CouchDBHelper couchDBHelper, string id, string rev = null) where T : class
        {
            GetEntityResponse<T> result;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {
                //Using anonymous entities
                var req = new GetEntityRequest(id, rev);
                result = await client.Entities.GetAsync<T>(req);

            }

            return result;
        }

        public async Task<AttachmentResponse> GetFileAsync(CouchDBHelper couchDBHelper, string id, string filename, string rev = null)
        {
            AttachmentResponse result;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {
                //Using anonymous entities
                result = await client.Attachments.GetAsync(id, rev, filename);

            }

            return result;
        }

        public async Task<EntityResponse<T>> InsertAsync<T>(CouchDBHelper couchDBHelper, T model) where T : class
        {
            EntityResponse<T> result;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {

                //Using anonymous entities
                result = await client.Entities.PostAsync(model);
            }

            return result;
        }

        public async Task<EntityResponse<T>> InsertWithFileAsync<T>(CouchDBHelper couchDBHelper, T model, List<AttachmentRequest> files) where T : class
        {
            EntityResponse<T> result;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {
                //Using anonymous entities
                var res = await client.Entities.PostAsync(model);

                // try to insert new attachments
                foreach (var f in files)
                {
                    string ctype = MimeMapping.MimeUtility.GetMimeMapping(f.FileName); // get content type
                    var req = new PutAttachmentRequest(res.Id, f.FileName, ctype, f.Contents);
                    await client.Attachments.PutAsync(request: req);
                }

                // get new result
                result = await client.Entities.GetAsync<T>(new GetEntityRequest(res.Id));

            }

            return result;
        }

        public async Task<ViewQueryResponse<T>> ViewQueryAsync<TRequest, T>(CouchDBHelper couchDBHelper, string designName, string viewName, TRequest keys, int limit, int page, bool reduce, bool desc) where T : class where TRequest : class
        {
            ViewQueryResponse<T> result;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {
                if (keys == null)
                {
                    var q = new QueryViewRequest(designName, viewName).Configure(qry => qry
                    .Limit(limit).Skip(page)
                    .Reduce(reduce)
                    .Descending(desc));


                    result = await client.Views.QueryAsync<T>(q);
                }
                else
                {
                    var q = new QueryViewRequest(designName, viewName).Configure(qry => qry
                    .Keys(keys)
                    .Limit(limit).Skip(page)
                    .Reduce(reduce)
                    .Descending(desc));

                    result = await client.Views.QueryAsync<T>(q);
                }


            }

            return result;
        }

        public async Task<ViewQueryResponse<T>> ViewQueryAsync<T>(CouchDBHelper couchDBHelper, string designName, string viewName, string key, int limit, int page, bool reduce, bool desc) where T : class
        {
            ViewQueryResponse<T> result;
            using (var client = new MyCouchClient(couchDBHelper.ServerAddr, couchDBHelper.DbName))
            {
                if (key == "none")
                {
                    var q = new QueryViewRequest(designName, viewName).Configure(qry => qry
                    .Limit(limit).Skip(page)
                    .Reduce(reduce)
                    .Descending(desc));

                    result = await client.Views.QueryAsync<T>(q);
                }
                else
                {
                    var q = new QueryViewRequest(designName, viewName).Configure(qry => qry
                    .Key(key)
                    .Limit(limit).Skip(page)
                    .Reduce(reduce)
                    .Descending(desc));

                    result = await client.Views.QueryAsync<T>(q);
                }
            }

            return result;
        }
    }
}
