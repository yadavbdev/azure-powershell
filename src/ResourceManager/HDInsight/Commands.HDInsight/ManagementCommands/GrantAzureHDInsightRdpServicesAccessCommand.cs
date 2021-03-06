﻿// ----------------------------------------------------------------------------------
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

using System;
using System.Management.Automation;
using Microsoft.Azure.Commands.HDInsight.Commands;
using Microsoft.Azure.Management.HDInsight.Models;
using Microsoft.WindowsAzure.Commands.Common;

namespace Microsoft.Azure.Commands.HDInsight
{
    [Cmdlet(
        VerbsSecurity.Grant,
        Constants.CommandNames.AzureHDInsightRdpServicesAccess),
    OutputType(
        typeof(void))]
    public class GrantAzureHDInsightRdpServicesAccessCommand : HDInsightCmdletBase
    {
        #region Input Parameter Definitions

        [Parameter(
            Position = 0,
            Mandatory = true,
            HelpMessage = "Gets or sets the name of the resource group.")]
        public string ResourceGroupName { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = true,
            HelpMessage = "Gets or sets the name of the cluster.")]
        public string ClusterName { get; set; }

        [Parameter(Position = 2,
            Mandatory = true,
            HelpMessage = "Gets or sets the credential for RDP access to the cluster.")]
        public PSCredential RdpCredential { get; set; }

        [Parameter(Position = 3,
            Mandatory = true,
            HelpMessage = "Gets or sets the expiry DateTime for RDP access on the cluster.")]
        public DateTime RdpAccessExpiry { get; set; }

        #endregion

        public override void ExecuteCmdlet()
        {
            var rdpParams = new RDPSettingsParameters
            {
                OsProfile = new OsProfile
                {
                    WindowsOperatingSystemProfile = new WindowsOperatingSystemProfile
                    {
                        RdpSettings = new RdpSettings
                        {
                            UserName = RdpCredential.UserName,
                            Password = RdpCredential.Password.ConvertToString(),
                            ExpiryDate = RdpAccessExpiry
                        }
                    }
                }
            };

            HDInsightManagementClient.ConfigureRdp(ResourceGroupName, ClusterName, rdpParams);
        }
    }
}
