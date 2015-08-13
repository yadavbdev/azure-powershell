using Microsoft.Azure.Management.RemoteApp;
using Microsoft.Azure.Management.RemoteApp.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.RemoteApp.Common
{
    public partial class RemoteAppManagementClientWrapper
    {
        internal IList<SecurityPrincipalInfo> GetUsers(string ResourceGroupName, string collectionName, string UserUpn)
        {
            SecurityPrincipalInfoListResult response = Client.CollectionOperations.GetUsers(ResourceGroupName, DefaultRemoteAppArmNamespace, UserUpn, RemoteAppApiVersionValue, null);

            return response.UserConsentStatuses;
        }

        internal SecurityPrincipalOperationErrorDetails AddUser(string ResourceGroupName, string collectionName, SecurityPrincipal userToAdd)
        {
            return Client.CollectionOperations.AddSecurityPrincipal(ResourceGroupName, DefaultRemoteAppArmNamespace, collectionName, userToAdd.Name, RemoteAppApiVersionValue, userToAdd);
        }

        internal SecurityPrincipalOperationErrorDetails DeleteUser(string ResourceGroupName, string collectionName, SecurityPrincipal userToDelete)
        {
            return Client.CollectionOperations.DeleteSecurityPrincipal(ResourceGroupName, DefaultRemoteAppArmNamespace, collectionName, userToDelete.Name, RemoteAppApiVersionValue, userToDelete);
        }

    }
}
