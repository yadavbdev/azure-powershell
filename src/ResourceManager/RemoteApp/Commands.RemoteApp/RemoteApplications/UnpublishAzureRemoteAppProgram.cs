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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using Microsoft.Azure.Commands.RemoteApp;

namespace Microsoft.Azure.Commands.RemoteApp.Cmdlet
{
    [Cmdlet(VerbsData.Unpublish, "AzureRemoteAppProgram", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High), OutputType(typeof(PublishingOperationResult))]
    public class UnpublishAzureRemoteAppProgram : RemoteAppArmResourceCmdletBase
    {
        [Parameter(Mandatory = true,
            Position = 1,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "RemoteApp collection name")]
        [ValidatePattern(NameValidatorString)]
        [Alias("Name")]
        public string CollectionName { get; set; }

        [Parameter(Mandatory = false,
            Position = 2,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Aliases of the programs to unpublish")]
        [ValidateNotNullOrEmpty()]
        public string[] Alias { get; set; }

        public override void ExecuteRemoteAppCmdlet()
        {
            IList<PublishingOperationResult> response = null;

            if (Alias.Count() == 0)
            {
#if false   // TODO need to add the resource strings
                if (ShouldProcess(Commands_RemoteApp.UnpublishProgramConfirmationDescription,
                    Commands_RemoteApp.GenericAreYouSureQuestion,
                    Commands_RemoteApp.UnpublishProgramCaptionMessage))
#endif
                {
                    response = RemoteAppClient.UnpublishAllApps(ResourceGroupName, CollectionName);
                }
            }
            else
            {
                AliasesListParameter appAlias = new AliasesListParameter()
                {
                    AliasesList = new List<string>(Alias)
                };

                response = RemoteAppClient.UnpublishApp(ResourceGroupName, CollectionName, appAlias);
            }

            if (response != null)
            {
                WriteObject(response, true);
            }
        }
    }
}
