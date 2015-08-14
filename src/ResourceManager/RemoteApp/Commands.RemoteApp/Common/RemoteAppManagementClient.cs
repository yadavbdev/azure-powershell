using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Management.RemoteApp;
using Microsoft.Azure.Management.RemoteApp.Models;
using Microsoft.WindowsAzure.Commands.ServiceManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Azure.Commands.RemoteApp.Common
{


    public partial class RemoteAppManagementClientWrapper
    {
        public const string DefaultRemoteAppArmNamespace = "Microsoft.RemoteApp";

        public const string RemoteAppApiVersionValue = "2014-09-01";

        private IRemoteAppManagementClient Client { get; set; }

        internal RemoteAppManagementClientWrapper(AzureProfile profile, AzureSubscription subscription)
        {
            Client = AzureSession.ClientFactory.CreateArmClient<RemoteAppManagementClient>(profile.Context, AzureEnvironment.Endpoint.ResourceManager);
        }

        #region Collections

        internal IEnumerable<Collection> ListCollections(string groupName)
        {
            IEnumerable<Collection> response = Client.Collection.List(groupName, DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            return response;
        }

        internal Collection Get(string ResourceGroupName, string collectionName)
        {
            Collection response = Client.Collection.Get(ResourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);

            return response;
        }

        internal CollectionCreationDetailsWrapper CreateOrUpdateCollection(string ResourceGroupName, string collectionName, CollectionCreationDetailsWrapper createDetails)
        {
            CollectionCreationDetailsWrapper response = Client.Collection.CreateOrUpdate(ResourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue, createDetails);



            return response;
        }

        internal void DeleteCollection(string ResourceGroupName, string collectionName)
        {
            Client.Collection.Delete(ResourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);
        }

        #endregion

        #region Accounts

        internal GetRemoteAppAccount GetAccount()
        {
            GetRemoteAppAccount response = Client.Account.GetAccountInfo(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            return response;
        }
        internal IEnumerable<Microsoft.Azure.Management.RemoteApp.Models.Location> GetLocations()
        {
            LocationPropertiesWrapper response = Client.Account.Locations(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            return response.Locations;
        }

        internal IEnumerable<RemoteAppBillingPlan> GetBillingPlans()
        {
            BillingPlanPropertiesWrapper response = Client.Account.Plans(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            return response.Plans;
        }

        internal bool SetAccount(RemoteAppAccountDetails accountInfo)
        {
            bool accountExists = false;

            GetRemoteAppAccount details = Client.Account.GetAccountInfo(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            if (details != null)
            {
                accountExists = true;

                if (!((details.WorkspaceName == accountInfo.WorkspaceName) && (details.PrivacyUrl == accountInfo.PrivacyUrl)))
                {
                    if (String.IsNullOrEmpty(accountInfo.WorkspaceName))
                    {
                        accountInfo.WorkspaceName = details.WorkspaceName;
                    }

                    if (accountInfo.PrivacyUrl == null)
                    {
                        accountInfo.PrivacyUrl = details.PrivacyUrl;
                    }

                    UpdateRemoteAppAccount accountUpdate = new UpdateRemoteAppAccount
                    {
                        Location = details.Location,
                        IsDesktopEnabled = details.IsDesktopEnabled,
                        MaxPublishedAppsPerService = details.MaxPublishedAppsPerService,
                        MaxServices = details.MaxServices,
                        MaxUsersPerService = details.MaxUsersPerService,
                        PrivacyUrl = details.PrivacyUrl,
                        RdWebUrl = details.RdWebUrl,
                        WorkspaceName = details.WorkspaceName,
                        Tags = new Dictionary<string, string>()
                    };

                    Client.Account.UpdateAccount(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue, accountUpdate);
                }
            }
            return accountExists;
        }

        internal void SetAccountBilling()
        {
            Client.Account.ActivateAccountBilling(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);
        }

        #endregion

        #region Sessions

        internal IEnumerable<SessionWrapper> GetSessionList(string resourceGroupName, string collectionName)
        {
            IEnumerable<SessionWrapper> response = Client.Collection.SessionList(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);

            return response;
        }

        internal void SetSessionLogoff(string resourceGroupName, string collectionName,
            string userUpn)
        {
            Client.Collection.SessionLogOff(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, userUpn, RemoteAppApiVersionValue);
        }

        internal void SetSessionDisconnect(string resourceGroupName, string collectionName,
            string userUpn)
        {
            Client.Collection.SessionDisconnect(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, userUpn, RemoteAppApiVersionValue);
        }

        internal void SetSessionSendMessage(string resourceGroupName, string collectionName, string userUpn,
            SessionSendMessageCommandParameter messageDetails)
        {
            Client.Collection.SessionSendMessage(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, userUpn, RemoteAppApiVersionValue, messageDetails);
        }

        #endregion
    }
}
