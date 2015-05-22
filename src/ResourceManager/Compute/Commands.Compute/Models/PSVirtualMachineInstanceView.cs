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

using Microsoft.Azure.Management.Compute.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.Compute.Models
{
    public class PSVirtualMachineInstanceView
    {
        public string ResourceGroupName { get; set; }

        public string Name { get; set; }

        public IList<DiskInstanceView> Disks { get; set; }

        [JsonIgnore]
        public string DisksText
        {
            get { return JsonConvert.SerializeObject(Disks, Formatting.Indented); }
        }

        public IList<VirtualMachineExtensionInstanceView> Extensions { get; set; }

        [JsonIgnore]
        public string ExtensionsText
        {
            get { return JsonConvert.SerializeObject(Extensions, Formatting.Indented); }
        }

        public int? PlatformFaultDomain { get; set; }

        public int? PlatformUpdateDomain { get; set; }

        public string RemoteDesktopThumbprint { get; set; }

        public VirtualMachineAgentInstanceView VMAgent { get; set; }

        [JsonIgnore]
        public string VMAgentText
        {
            get { return JsonConvert.SerializeObject(VMAgent, Formatting.Indented); }
        }

        public IList<InstanceViewStatus> Statuses { get; set; }

        [JsonIgnore]
        public string StatusesText
        {
            get { return JsonConvert.SerializeObject(Statuses, Formatting.Indented); }
        }
    }

    public static class PSVirtualMachineInstanceViewExtension
    {
        public static PSVirtualMachineInstanceView ToPSVirtualMachineInstanceView(
            this VirtualMachineInstanceView virtualMachineInstanceView,
            string resourceGroupName = null,
            string vmName = null)
        {
            PSVirtualMachineInstanceView result = new PSVirtualMachineInstanceView
            {
                ResourceGroupName = resourceGroupName,
                Name = vmName,
                Disks = virtualMachineInstanceView.Disks,
                Extensions = virtualMachineInstanceView.Extensions,
                Statuses = virtualMachineInstanceView.Statuses,
                PlatformFaultDomain = virtualMachineInstanceView.PlatformFaultDomain,
                PlatformUpdateDomain = virtualMachineInstanceView.PlatformUpdateDomain,
                RemoteDesktopThumbprint = virtualMachineInstanceView.RemoteDesktopThumbprint,
                VMAgent = virtualMachineInstanceView.VMAgent
            };

            return result;
        }

        public static PSVirtualMachineInstanceView ToPSVirtualMachineInstanceView(
            this VirtualMachineGetResponse response,
            string resourceGroupName = null,
            string vmName = null)
        {
            if (response == null)
            {
                return null;
            }

            return response.VirtualMachine.InstanceView.ToPSVirtualMachineInstanceView(resourceGroupName, vmName);
        }
    }
}
