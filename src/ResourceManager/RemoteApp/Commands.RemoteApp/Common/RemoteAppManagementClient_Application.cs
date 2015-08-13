using Microsoft.Azure.Management.RemoteApp;
using Microsoft.Azure.Management.RemoteApp.Model;
using System.Collections.Generic;

namespace Microsoft.Azure.Commands.RemoteApp.Common
{
    public partial class RemoteAppManagementClientWrapper
    {
        internal PublishedApplicationDetails GetApplication(string resourceGroupName, string collectionName, string alias)
        {
            GetPublishedApplicationResult response = Client.Collection.GetPublishedApp(resourceGroupName, collectionName, alias);

            if (response != null)
            {
                return response.Result;
            }
            else
            {
                return null;
            }
        }

        internal IList<PublishedApplicationDetails> GetApplications(string resourceGroupName, string collectionName)
        {
            GetPublishedApplicationListResult response = Client.Collection.ListPublishedApp(resourceGroupName, collectionName);

            if (response != null)
            {
                return response.ResultList;
            }
            else
            {
                return null;
            }
        }

        internal IList<StartMenuApplication> GetStartMenuApps(string resourceGroupName, string collectionName)
        {
            GetStartMenuApplicationListResult response = Client.Collection.ListStartMenuApps(resourceGroupName, collectionName);

            if (response != null)
            {
                return response.ResultList;
            }
            else
            {
                return null;
            }
        }

        internal StartMenuApplication GetStartMenuApp(string resourceGroupName, string collectionName, string applicationId)
        {
            GetStartMenuApplicationResult response = Client.Collection.GetStartMenuApp(resourceGroupName, collectionName, applicationId);

            if (response != null)
            {
                return response.Result;
            }
            else
            {
                return null;
            }
        }

        internal  IList<PublishingOperationResult> PublishApp(string resourceGroupName, string collectionName, ApplicationDetailsListParameter details)
        {
            PublishApplicationsResult response = Client.Collection.PublishApplications(resourceGroupName, collectionName, details);

            if (response != null)
            {
                return response.ResultList;
            }
            else
            {
                return null;
            }
        }

        internal IList<PublishingOperationResult> UnpublishApp(string resourceGroupName, string collectionName, AliasesListParameter details)
        {
            UnpublishApplicationsResult response = Client.Collection.Unpublish(resourceGroupName, collectionName, details);

            if (response != null)
            {
                return response.ResultList;
            }
            else
            {
                return null;
            }
        }

        internal IList<PublishingOperationResult> UnpublishAllApps(string resourceGroupName, string collectionName)
        {
            UnpublishApplicationsResult response = Client.Collection.UnpublishAll(resourceGroupName, collectionName);

            if (response != null)
            {
                return response.ResultList;
            }
            else
            {
                return null;
            }
        }
    }
}
