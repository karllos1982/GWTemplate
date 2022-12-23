using GW.Core;
using GW.Common;
using GW.ApplicationHelpers;
using System.Text;

namespace Template.Core.Manager
{
    public class TemplateMailCenter : MailManager
    {
        public MailSettings Settings { get; set; }

        public TemplateMailCenter(ISettings settings)
        {
            Settings = settings.MailSettings;
            Settings.ContentEncoding = System.Text.Encoding.UTF8;
        }

        public void Initialize()
        {

        }



        public OperationStatus SendTemporaryPassword(string email, string name, string code)
        {
            OperationStatus ret = new OperationStatus(true);
            StringBuilder strq = new StringBuilder();
            strq.Append("<b>SOLICITAÇÃO DE SENHA TEMPORÁRIA</b>" + "<BR/> <BR/>");
            strq.Append("Esta é a sua senha temporária: " + "<BR/>");
            strq.Append("<b>" + code + "</b><BR/>");
            strq.Append("Utilize essa senha pra acessar o site. ");
            strq.Append("Após acessar, recomendamos que você redefina sua senha.");
           
            ret.Status = Send(name, email, Settings.NameSender + " - Recuperação de Senha", strq.ToString());

            return ret;
        }

        public OperationStatus SendActiveAccountCode(string email, string name, string code)
        {
            OperationStatus ret = new OperationStatus(true);
            StringBuilder strq = new StringBuilder();

            strq.Append("<b>ATIVAÇÃO DE CONTA</b>" + "<BR/> <BR/>");
            strq.Append("Este é o código para a ativação de conta:" + "<BR/>");
            strq.Append("<b>" + code + "</b><BR/>");
            strq.Append("Acesse o site e utilize esse código pra ativação da conta.");
          
            ret.Status = Send(name, email, Settings.NameSender + " - Ativação de Conta", strq.ToString());

            return ret;
        }

        public OperationStatus SendChangePassowordCode(string email, string name, string code)
        {
            OperationStatus ret = new OperationStatus(true);
            StringBuilder strq = new StringBuilder();
            strq.Append("<b>REDEFINIÇÃO DE SENHA</b>" + "<BR/> <BR/>");
            strq.Append("Este é o código de autorização para a troca da senha:" + "<BR/>");
            strq.Append("<b>" + code + "</b><BR/>");
            strq.Append("Acesse o site e utilize esse código pra trocar a sua senha.");
            
            ret.Status = Send(name, email, Settings.NameSender + " - Redefinição de Senha", strq.ToString());

            return ret;
        }

        public OperationStatus SendEmailConfirmationCode(string email, string name, string code)
        {
            OperationStatus ret = new OperationStatus(true);
            StringBuilder strq = new StringBuilder();
            strq.Append("<b>CONFIRMAÇÃO DE E-MAIL</b>" + "<BR/> <BR/>");
            strq.Append("Bem-vindo ao Portal " + Settings.NameSender + "<BR/>");
            strq.Append("Utilize o código abaixo para confirmar seu cadastro no site o Portal." + "<BR/>");
            strq.Append("<b>" + code + "</b><BR/>");
            strq.Append("Prossiga com a confirmação do cadastro no site.");

            ret.Status = Send(name, email, Settings.NameSender + " - Confirmação de Cadastro", strq.ToString());

            return ret;
        }

    }
}
