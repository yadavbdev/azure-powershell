using Microsoft.Azure.Management.RemoteApp;
using Microsoft.Azure.Management.RemoteApp.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.RemoteApp.Common
{
    public partial class RemoteAppManagementClientWrapper
    {
        internal PublishedApplicationDetails GetApplication(string resourceGroupName, string collectionName, string alias)
        {
            return Client.Collection.GetPublishedApp(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, alias, RemoteAppApiVersionValue);

        }

        internal IList<PublishedApplicationDetails> GetApplications(string resourceGroupName, string collectionName)
        {
            return Client.Collection.ListPublishedApp(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);
        }

        internal IList<StartMenuApplication> GetStartMenuApps(string resourceGroupName, string collectionName)
        {
            return Client.Collection.ListStartMenuApps(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);
        }

        internal StartMenuApplication GetStartMenuApp(string resourceGroupName, string collectionName, string applicationId)
        {
            return Client.Collection.GetStartMenuApp(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, applicationId, RemoteAppApiVersionValue);

        }

        internal PublishingOperationResult PublishApp(string resourceGroupName, string collectionName, ApplicationDetails details)
        {
            return Client.Collection.PublishOrUpdateApplication(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, details.Alias, RemoteAppApiVersionValue, details);

        }

        internal PublishingOperationResult UnpublishApp(string resourceGroupName, string collectionName, string alias)
        {
            return Client.Collection.Unpublish(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue, alias);

        }
    }
}
