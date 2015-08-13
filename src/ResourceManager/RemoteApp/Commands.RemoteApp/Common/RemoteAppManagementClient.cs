using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Management.RemoteApp;
using Microsoft.Azure.Management.RemoteApp.Model;
using Microsoft.WindowsAzure.Commands.ServiceManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Azure.Commands.RemoteApp.Common
{
    public partial class RemoteAppManagementClientWrapper
    {
        private RemoteAppManagementClient Client { get; set; }

        internal RemoteAppManagementClientWrapper(AzureProfile profile, AzureSubscription subscription)
        {
            Client = AzureSession.ClientFactory.CreateClient<RemoteAppManagementClient>(profile, subscription, AzureEnvironment.Endpoint.ResourceManager);
        }

        #region Collections

        internal IEnumerable<Collection> ListCollections(string groupName)
        {
            ListCollectionOperationResult response = Client.Collection.List(groupName);

            return response.Collections;
        }

        internal Collection Get(string ResourceGroupName, string collectionName)
        {
            GetCollectionOperationResult response = Client.Collection.Get(ResourceGroupName, collectionName);

            return response.Collection;
        }

        internal CollectionCreationDetails CreateOrUpdateCollection(string ResourceGroupName, string collectionName, CollectionCreationDetails createDetails)
        {
            CreateCollectionOperationResult response = Client.Collection.CreateOrUpdate(ResourceGroupName, collectionName, createDetails);

            return response.Collection.Properties;
        }

        #endregion

        #region Accounts

        internal RemoteAppAccountDetails GetAccount()
        {
            GetAccountOperationResult response = Client.Account.GetAccountInfo();

            return response.Account.AccountDetails;
        }

        internal IEnumerable<string> GetEnabledFeatures()
        {
            GetEnabledFeaturesOperationResult response = Client.Account.Features();

            return response.EnabledFeatures.FeaturesProperties.FeaturesList;
        }

        internal IEnumerable<Microsoft.Azure.Management.RemoteApp.Model.Location> GetLocations()
        {
            GetAccountLocationOperationResult response = Client.Account.Locations();

            return response.LocationResource.LocationProperties.LocationList;
        }

        internal IEnumerable<RemoteAppBillingPlan> GetBillingPlans() 
        {
            GetAccountBillingPlanOperationResult response = Client.Account.Plans();

            return response.BillingPlanResource.BillingPlansProperties.BillingPlans;
        }

        internal bool SetAccount(RemoteAppAccountDetails accountInfo)
        {
            bool accountExists = false;
            
            RemoteAppAccountDetails details = Client.Account.GetAccountInfo().Account.AccountDetails;

            if (details != null)
            {
                accountExists = true;

                if(!((details.WorkspaceName == accountInfo.WorkspaceName) && (details.PrivacyUrl == accountInfo.PrivacyUrl))) 
                {
                    if (String.IsNullOrEmpty(accountInfo.WorkspaceName))
                    {
                        accountInfo.WorkspaceName = details.WorkspaceName;
                    }

                    if (accountInfo.PrivacyUrl == null)
                    {
                        accountInfo.PrivacyUrl = details.PrivacyUrl;
                    }

                    UpdateRemoteAppAccount accountUpdate = new UpdateRemoteAppAccount{
                        Location = details.Location,
                        UpdatedAccountDetails = accountInfo,
                        Tags = new Dictionary<string, string>()
                    }; 

                    Client.Account.UpdateAccount(accountUpdate);
                }
            }
            return accountExists;
        }

        internal void SetAccountBilling()
        {
            Client.Account.ActivateAccountBilling();
        }

        #endregion

        #region Sessions

        internal IEnumerable<SessionWrapper> GetSessionList(string resourceGroupName, string collectionName)
        {
            GetSessionListResult response = Client.Collection.SessionList(resourceGroupName, collectionName);

            return response.SessionList;
        }

        internal void SetSessionLogoff(string resourceGroupName, string collectionName,
            string userUpn)
        {
            Client.Collection.SessionLogOff(resourceGroupName, collectionName, userUpn);
        }

        internal void SetSessionDisconnect(string resourceGroupName, string collectionName,
            string userUpn)
        {
            Client.Collection.SessionDisconnect(resourceGroupName, collectionName, userUpn);
        }

        internal void SetSessionSendMessage(string resourceGroupName, string collectionName, string userUpn,
            SessionSendMessageCommandParameter messageDetails)
        {
            Client.Collection.SessionSendMessage(resourceGroupName, collectionName, userUpn, messageDetails);
        }

        #endregion
    }
}
