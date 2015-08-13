// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Management.RemoteApp.Model;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.RemoteApp.Cmdlet
{
    public abstract class SecurityPrincipalsCmdletBase : RemoteAppArmResourceCmdletBase
    {
        public enum Operation
        {
            Add,
            Remove
        }

        public SecurityPrincipalOperationsResult ProcessUsers(Operation op, string collectionName, PrincipalProviderType provider, string[] userUpn)
        {
            SecurityPrincipalOperationsResult response = null;
            SecurityPrincipalListParameter spList = null;

            spList = BuildUserList(userUpn, provider);

            if (op == Operation.Add)
            {
                response = RemoteAppClient.AddUsers(ResourceGroupName, collectionName, spList);
            }
            else
            {
                response = RemoteAppClient.DeleteUsers(ResourceGroupName, collectionName, spList);
            }

            return response;
        }

        public SecurityPrincipalListParameter BuildUserList(string[] Users, PrincipalProviderType userIdType)
        {
            SecurityPrincipalListParameter userList = new SecurityPrincipalListParameter();
            List<SecurityPrincipal> spList = new List<SecurityPrincipal>();

            foreach (string user in Users)
            {
                SecurityPrincipal principal = new SecurityPrincipal()
                {
                    AadObjectId = null,
                    Description = null,
                    Name = user,
                    SecurityPrincipalType = PrincipalType.User,
                    UserIdType = userIdType
                };

                spList.Add(principal);
            }

            userList.SecurityPrincipals = spList;

            return userList;
        }
    }
}
