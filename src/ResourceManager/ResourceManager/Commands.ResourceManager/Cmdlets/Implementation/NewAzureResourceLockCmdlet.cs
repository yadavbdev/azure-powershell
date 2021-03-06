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

namespace Microsoft.Azure.Commands.ResourceManager.Cmdlets.Implementation
{
    using System.Management.Automation;
    using Microsoft.Azure.Commands.ResourceManager.Cmdlets.Entities.Locks;
    using Microsoft.Azure.Commands.ResourceManager.Cmdlets.Extensions;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The new azure resource lock cmdlet.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AzureResourceLock", SupportsShouldProcess = true, DefaultParameterSetName = ResourceLockManagementCmdletBase.SubscriptionResourceLevelLock), OutputType(typeof(PSObject))]
    public class NewAzureResourceLockCmdlet : ResourceLockManagementCmdletBase
    {
        /// <summary>
        /// Gets or sets the extension resource name parameter.
        /// </summary>
        [Alias("ExtensionResourceName")]
        [Parameter(ParameterSetName = ResourceLockManagementCmdletBase.ResourceGroupLevelLock, Mandatory = true, ValueFromPipelineByPropertyName = false, HelpMessage = "The name of the lock.")]
        [Parameter(ParameterSetName = ResourceLockManagementCmdletBase.ResourceGroupResourceLevelLock, Mandatory = true, ValueFromPipelineByPropertyName = false, HelpMessage = "The name of the lock.")]
        [Parameter(ParameterSetName = ResourceLockManagementCmdletBase.ScopeLevelLock, Mandatory = true, ValueFromPipelineByPropertyName = false, HelpMessage = "The name of the lock.")]
        [Parameter(ParameterSetName = ResourceLockManagementCmdletBase.SubscriptionLevelLock, Mandatory = true, ValueFromPipelineByPropertyName = false, HelpMessage = "The name of the lock.")]
        [Parameter(ParameterSetName = ResourceLockManagementCmdletBase.SubscriptionResourceLevelLock, Mandatory = true, ValueFromPipelineByPropertyName = false, HelpMessage = "The name of the lock.")]
        [Parameter(ParameterSetName = ResourceLockManagementCmdletBase.TenantResourceLevelLock, Mandatory = true, ValueFromPipelineByPropertyName = false, HelpMessage = "The name of the lock.")]
        [ValidateNotNullOrEmpty]
        public string LockName { get; set; }

        /// <summary>
        /// Gets or sets the extension resource name parameter.
        /// </summary>
        [Alias("Level")]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = false, HelpMessage = "The level of the lock.")]
        [ValidateNotNullOrEmpty]
        public LockLevel LockLevel { get; set; }

        /// <summary>
        /// Gets or sets the extension resource name parameter.
        /// </summary>
        [Alias("Notes")]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = false, HelpMessage = "The notes of the lock.")]
        [ValidateNotNullOrEmpty]
        public string LockNotes { get; set; }

        /// <summary>
        /// Gets or sets the force parameter.
        /// </summary>
        [Parameter(Mandatory = false, HelpMessage = "Do not ask for confirmation.")]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// Executes the cmdlet.
        /// </summary>
        protected override void OnProcessRecord()
        {
            base.OnProcessRecord();
            var resourceId = this.GetResourceId(this.LockName);
            this.ConfirmAction(
                this.Force,
                this.GetActionMessage(resourceId),
                this.GetProccessMessage(),
                resourceId,
                () =>
                {
                    var apiVersion = this.DetermineApiVersion(resourceId: resourceId).Result;
                    var resourceBody = this.GetResourceBody();

                    var operationResult = this.GetResourcesClient()
                        .PutResource(
                            resourceId: resourceId,
                            apiVersion: apiVersion,
                            resource: resourceBody,
                            cancellationToken: this.CancellationToken.Value)
                        .Result;

                    var managementUri = this.GetResourcesClient()
                      .GetResourceManagementRequestUri(
                          resourceId: resourceId,
                          apiVersion: apiVersion);

                    var activity = string.Format("PUT {0}", managementUri.PathAndQuery);
                    var result = this.GetLongRunningOperationTracker(activityName: activity, isResourceCreateOrUpdate: true)
                        .WaitOnOperation(operationResult: operationResult);

                    this.WriteObject(this.GetOutputObjects(result.ToJToken()), enumerateCollection: true);
                });
        }

        /// <summary>
        /// Gets the action message.
        /// </summary>
        /// <param name="resourceId">The resource Id.</param>
        protected virtual string GetActionMessage(string resourceId)
        {
            return "Are you sure you want to create the following lock: " + resourceId;
        }

        /// <summary>
        /// Gets the process message.
        /// </summary>
        protected virtual string GetProccessMessage()
        {
            return "Creating the lock.";
        }

        /// <summary>
        /// Gets the lock.
        /// </summary>
        private JToken GetResourceBody()
        {
            var lockObject = new Lock
            {
                Properties = new LockProperties
                {
                    Level = this.LockLevel,
                    Notes = this.LockNotes,
                }
            };

            return lockObject.ToJToken();
        }
    }
}