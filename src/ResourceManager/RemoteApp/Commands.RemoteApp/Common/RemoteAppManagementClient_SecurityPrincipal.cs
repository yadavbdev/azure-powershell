using Microsoft.Azure.Management.RemoteApp;
using Microsoft.Azure.Management.RemoteApp.Model;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.RemoteApp.Common
{
    public partial class RemoteAppManagementClientWrapper
    {
        internal IList<SecurityPrincipalInfo> GetUsers(string ResourceGroupName, string collectionName, string UserUpn)
        {
            SecurityPrincipalInfoListResult response = Client.Collection.GetUsers(ResourceGroupName, UserUpn, null);

            return response.ResultList;
        }


        internal SecurityPrincipalOperationsResult AddUsers(string ResourceGroupName, string collectionName, SecurityPrincipalListParameter spList)
        {
            return Client.Collection.AddSecurityPrincipals(ResourceGroupName, collectionName, spList);
        }

        internal SecurityPrincipalOperationsResult DeleteUsers(string ResourceGroupName, string collectionName, SecurityPrincipalListParameter spList)
        {
            return Client.Collection.DeleteSecurityPrincipals(ResourceGroupName, collectionName, spList);
        }

    }
}
