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
            Client = AzureSession.ClientFactory.CreateArmClient<RemoteAppManagementClient>();
        }

        #region Collections

        internal IEnumerable<Collection> ListCollections(string groupName)
        {
            IEnumerable<Collection> response = Client.CollectionOperations.List(groupName, DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            return response;
        }

        internal Collection Get(string ResourceGroupName, string collectionName)
        {
            Collection response = Client.CollectionOperations.Get(ResourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);

            return response;
        }

        internal CollectionCreationDetails CreateOrUpdateCollection(string ResourceGroupName, string collectionName, CollectionCreationDetailsWrapper createDetails)
        {
            CollectionCreationDetailsWrapper response = Client.CollectionOperations.CreateOrUpdate(ResourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue, createDetails);

            return response.Properties;
        }

        internal void DeleteCollection(string ResourceGroupName, string collectionName)
        {
            Client.CollectionOperations.Delete(ResourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);
        }

        #endregion

        #region Accounts

        internal RemoteAppAccountDetails GetAccount()
        {
            GetRemoteAppAccount response = Client.Account.GetAccountInfo(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            return response.Properties;
        }

        internal IEnumerable<string> GetEnabledFeatures()
        {
            EnabledFeaturesPropertiesWrapper response = Client.Account.Features(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            return response.Properties.Features;
        }

        internal IEnumerable<Microsoft.Azure.Management.RemoteApp.Models.Location> GetLocations()
        {
            LocationPropertiesWrapper response = Client.Account.Locations(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            return response.Properties.Locations;
        }

        internal IEnumerable<RemoteAppBillingPlan> GetBillingPlans()
        {
            BillingPlanPropertiesWrapper response = Client.Account.Plans(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue);

            return response.Properties.Plans;
        }

        internal bool SetAccount(RemoteAppAccountDetails accountInfo)
        {
            bool accountExists = false;

            RemoteAppAccountDetails details = Client.Account.GetAccountInfo(DefaultRemoteAppArmNamespace, RemoteAppApiVersionValue).Properties;

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
                        Properties = accountInfo,
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
            IEnumerable<SessionWrapper> response = Client.CollectionOperations.SessionList(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, RemoteAppApiVersionValue);

            return response;
        }

        internal void SetSessionLogoff(string resourceGroupName, string collectionName,
            string userUpn)
        {
            Client.CollectionOperations.SessionLogOff(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, userUpn, RemoteAppApiVersionValue);
        }

        internal void SetSessionDisconnect(string resourceGroupName, string collectionName,
            string userUpn)
        {
            Client.CollectionOperations.SessionDisconnect(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, userUpn, RemoteAppApiVersionValue);
        }

        internal void SetSessionSendMessage(string resourceGroupName, string collectionName, string userUpn,
            SessionSendMessageCommandParameter messageDetails)
        {
            Client.CollectionOperations.SessionSendMessage(resourceGroupName, DefaultRemoteAppArmNamespace, collectionName, userUpn, RemoteAppApiVersionValue, messageDetails);
        }

        #endregion
    }
}
