using GW.Core.Manager;
using GW.Core.Common;

namespace Template.Core.Manager
{
    public interface ITemplateMailing: IMailingManager
    {

        OperationStatus SendTemporaryPassword(string email, string name, string code);

        OperationStatus SendActiveAccountCode(string email, string name, string code);

        OperationStatus SendChangePassowordCode(string email, string name, string code);

        OperationStatus SendEmailConfirmationCode(string email, string name, string code);

    }
}
