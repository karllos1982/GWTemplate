using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Threading;
using GW.Common;
using System.ComponentModel;
using System.IO;

namespace Template.API
{
    public class FileService
    {

        private BlobClient _service = null;

        private string _connection = "";
        private string _container = ""; 

        public FileService(string connection, string container)
        {
            _connection = connection;   
            _container = container; 
           

        }

        public async Task<OperationStatus> UploadFile(Stream content, string filename)
        {

            OperationStatus ret = new OperationStatus(true);

            try
            {
                _service = new BlobClient(_connection, _container,filename);

                BlobContentInfo info = null;

                info = await _service.UploadAsync(content);
                ret.Returns = _service.Uri.AbsoluteUri;
                
            }
            catch (Exception ex)
            {
                ret.Status = false;
                ret.Error = ex; 
            }
            
            return ret;

        }

        public Stream GetFile(string filename)
        {

            Stream ret = null; 

            try
            {
                _service = new BlobClient(_connection, _container, filename);
                
                BlobOpenReadOptions opts = new BlobOpenReadOptions(false);
             
                if (!_service.Exists())
                {                                   
                    _service = new BlobClient(_connection, _container, "user_anonymous.png");                    
                }

                ret = _service.OpenRead(opts);
                

            }
            catch (Exception ex)
            {
            
            }


            return ret;

        }
    }
}
