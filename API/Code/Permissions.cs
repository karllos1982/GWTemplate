using GW.Common;

namespace Template.API
{
   

    public interface IUserPermissionsManager<T>
    {
        
        List<UserPermissions> GetPermissions();

        void SetPermissions(List<UserPermissions> permissions); 
    }

    public class UserPermissionsManager : IUserPermissionsManager<UserPermissions>
    {
        public UserPermissionsManager()
        {

        }

        private List<UserPermissions> _permissions; 

        public UserPermissionsManager(List<UserPermissions> permissions)
        {
            _permissions = permissions;
        }   

        public List<UserPermissions> GetPermissions()
        {
            return _permissions;
        }

        public void SetPermissions(List<UserPermissions> permissions)
        {
            _permissions = permissions;
        }
    }

}