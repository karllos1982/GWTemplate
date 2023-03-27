using System.Data.SqlClient;
using GW.Core;
using GW.Membership.Data;

namespace Core.Data
{
    public class ContextBuilder : IContextBuilder
    {
        public ISettings Settings { get; set; }

        public ContextBuilder(ISettings settings)
        {
            Settings = settings;
        }   

        public void BuilderContext(IContext context)
        {
            ((DapperContext)context)
                .Connection[0] = new SqlConnection(Settings.Sources[0].SourceValue);

            context.Begin(0, System.Data.IsolationLevel.ReadUncommitted);

        }
    }
}
