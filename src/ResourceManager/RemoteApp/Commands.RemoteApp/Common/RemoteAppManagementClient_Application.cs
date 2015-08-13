using Microsoft.Azure.Management.RemoteApp;
using Microsoft.Azure.Management.RemoteApp.Models;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.RemoteApp.Common
{
    public partial class RemoteAppManagementClientWrapper
    {
        internal PublishedApplicationDetails GetApplication(string resourceGroupName, string collectionName, string alias)
        {
            return Client.CollectionOperations.GetPublishedApp(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, alias, RemoteAppApiVersionValue);

        }

        internal IList<PublishedApplicationDetails> GetApplications(string resourceGroupName, string collectionName)
        {
            return Client.CollectionOperations.ListPublishedApp(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);
        }

        internal IList<StartMenuApplication> GetStartMenuApps(string resourceGroupName, string collectionName)
        {
            return Client.CollectionOperations.ListStartMenuApps(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);
        }

        internal StartMenuApplication GetStartMenuApp(string resourceGroupName, string collectionName, string applicationId)
        {
            return Client.CollectionOperations.GetStartMenuApp(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, applicationId, RemoteAppApiVersionValue);

        }

        internal PublishingOperationResult PublishApp(string resourceGroupName, string collectionName, ApplicationDetails details)
        {
            return Client.CollectionOperations.PublishOrUpdateApplication(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, details.Alias, RemoteAppApiVersionValue, details);

        }

        internal IList<PublishingOperationResult> UnpublishApp(string resourceGroupName, string collectionName, AliasesListParameter details)
        {
            return Client.CollectionOperations.Unpublish(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue, details);

        }

        internal IList<PublishingOperationResult> UnpublishAllApps(string resourceGroupName, string collectionName)
        {
            return Client.CollectionOperations.UnpublishAll(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);

        }
    }
}
