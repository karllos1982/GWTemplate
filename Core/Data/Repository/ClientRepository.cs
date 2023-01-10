using GW.Common;
using GW.Core;
using Template.Contracts;
using Template.Contracts.Data;
using Template.Models;
using GW.Membership.Data;

namespace Template.Data
{
    public class ClientRepository : IClientRepository
    {

        public ClientRepository(IContext context)
        {
            Context = context;
        }

        private ClientQueryBuilder query = new ClientQueryBuilder();

        public IContext Context { get; set; }

        public async Task Create(ClientEntry model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("Client", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
        }

        public async Task<ClientResult> Read(ClientParam param)
        {
            ClientResult ret = null;

            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context).ExecuteQueryFirstAsync<ClientResult>(sql, param);

            return ret;
        }

        public async Task Update(ClientEntry model)
        {

            string sql = query.QueryForUpdate("Client", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task Delete(ClientEntry model)
        {

            string sql = query.QueryForDelete("Client", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task<List<ClientList>> List(ClientParam param)
        {
            List<ClientList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<ClientList>(query.QueryForList(null), param);

            return ret;
        }

        public async Task<List<ClientResult>> Search(ClientParam param)
        {
            List<ClientResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<ClientResult>(query.QueryForSearch(null), param);


            return ret;
        }


    }

}
