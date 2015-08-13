using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.Azure.Commands.RemoteApp.Common;
using System.Management.Automation;
using System;
using Hyak.Common;

namespace Microsoft.Azure.Commands.RemoteApp.Cmdlet
{
    public abstract partial class RemoteAppArmCmdletBase : AzurePSCmdlet
    {
        public RemoteAppArmCmdletBase()
        {

        }

        private RemoteAppManagementClientWrapper _RemoteAppClient = null;

        public RemoteAppManagementClientWrapper RemoteAppClient
        {
            get
            {
                if (_RemoteAppClient == null)
                {
                    _RemoteAppClient = new RemoteAppManagementClientWrapper(Profile, Profile.Context.Subscription);
                }

                return _RemoteAppClient;
            }

            set
            {
                // for testing purpose only
                _RemoteAppClient = value;
            }
        }


        private void HandleCloudException(object targetObject, CloudException e)
        {
            CloudRecordState cloudRecord = RemoteAppCollectionErrorState.CreateErrorStateFromCloudException(e, String.Empty, targetObject);
            if (cloudRecord.state.type == ExceptionType.NonTerminating)
            {
                WriteError(cloudRecord.er);
            }
            else
            {
                ThrowTerminatingError(cloudRecord.er);
            }
        }
    }
}
