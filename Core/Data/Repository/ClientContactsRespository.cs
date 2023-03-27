using GW.Common;
using GW.Core;
using Template.Contracts.Data;
using Template.Models;
using GW.Membership.Data;

namespace Template.Data
{
    public class ClientContactsRespository : IClientContactsRepository
    {
        public ClientContactsRespository(IContext context)
        {
            Context = context;
        }

        private ClientContactsQueryBuilder query = new ClientContactsQueryBuilder();

        public IContext Context { get; set ; }

        public  async Task Create(ClientContactsEntry model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("ClientContacts", model, model);
            await((DapperContext)Context).ExecuteAsync(sql, model);
        }

        public async Task Delete(ClientContactsEntry model)
        {
            string sql = query.QueryForDelete("ClientContacts", model, model);
            await((DapperContext)Context).ExecuteAsync(sql, model);
        }

        public async Task<List<ClientContactsList>> List(ClientContactsParam param)
        {
            List<ClientContactsList> ret = null;

            ret = await((DapperContext)Context)
                .ExecuteQueryToListAsync<ClientContactsList>(query.QueryForList(null), param);

            return ret;
        }

        public async Task<ClientContactsResult> Read(ClientContactsParam param)
        {
            ClientContactsResult ret = null;

            string sql = query.QueryForGet(null);

            ret = await((DapperContext)Context).ExecuteQueryFirstAsync<ClientContactsResult>(sql, param);

            return ret;
        }

        public async Task<List<ClientContactsResult>> Search(ClientContactsParam param)
        {
            List<ClientContactsResult> ret = null;

            ret = await((DapperContext)Context)
                .ExecuteQueryToListAsync<ClientContactsResult>(query.QueryForSearch(null), param);


            return ret;
        }

        public async Task Update(ClientContactsEntry model)
        {
            string sql = query.QueryForUpdate("ClientContacts", model, model);
            await((DapperContext)Context).ExecuteAsync(sql, model);
        }
    }
}
