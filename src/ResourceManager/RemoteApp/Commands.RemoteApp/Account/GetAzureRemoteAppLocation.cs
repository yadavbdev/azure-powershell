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

using Microsoft.Azure.Management.RemoteApp.Models;
using Microsoft.Azure.Commands.RemoteApp.Common;
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace Microsoft.Azure.Commands.RemoteApp.Cmdlet
{
    [Cmdlet(VerbsCommon.Get, "AzureRemoteAppLocation")]
    public class GetAzureRemoteAppAccountLocations : RemoteAppArmCmdletBase
    {
        public override void ExecuteCmdlet()
        {
            IEnumerable<Location> locations = RemoteAppClient.GetAccount().LocationList;

            if (locations != null && locations.Any<Location>())
            {
                List<string> locationNames = new List<string>();

                foreach (Location loc in locations)
                {
                    locationNames.Add(loc.DisplayName);
                }

                WriteObject(locationNames, true);
            }
            else
            {
                WriteVerboseWithTimestamp(String.Format(Commands_RemoteApp.NoLocationsFound));
            }
        }
    }
}