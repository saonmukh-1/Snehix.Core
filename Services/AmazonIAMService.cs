using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Snehix.Core.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snehix.Core.API.Services
{
    public class AmazonIAMService
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly string AWSAccessKey = "AKIAR5JVQC2D7GCMNCW4";
        /// <summary>
        /// 
        /// </summary>
        public readonly string AWSSecurityKey = "Brh3jBc8514G1yrOYSKZeJ9jy71xQgLKv/LwysXb";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userName"></param>
        public async Task<AmazonAccountInfo> CreateIAMUser(string path, string userName)
        {
            var client = new AmazonIdentityManagementServiceClient(AWSAccessKey, AWSSecurityKey);
            var request = new CreateUserRequest
            {
                UserName = userName,
                Path = path
            };

            try
            {
                var response = await client.CreateUserAsync(request);
                // Assign the read-only policy to the new user
                await client.PutUserPolicyAsync(new PutUserPolicyRequest
                {
                    UserName = userName,
                    PolicyDocument = "{\"Version\":\"2012-10-17\",\"Statement\":{\"Effect\":\"Allow\",\"Action\":\"*\",\"Resource\":\"*\"}}",
                    PolicyName = "AllAccessPolicy"                    
                });

                var accessKey = await client.CreateAccessKeyAsync(new CreateAccessKeyRequest
                {
                    // Use the user created in the CreateUser example
                    UserName = userName
                });
                var result = new AmazonAccountInfo()
                {
                    AccessKey = accessKey.AccessKey.AccessKeyId,
                    SecurityKey = accessKey.AccessKey.SecretAccessKey
                };
                return result;
            }
            catch (EntityAlreadyExistsException)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> ListUsers()
        {
            var iamClient = new AmazonIdentityManagementServiceClient(AWSAccessKey, AWSSecurityKey);
            var requestUsers = new ListUsersRequest() { MaxItems = 10 };
            var responseUsers = await iamClient.ListUsersAsync(requestUsers);
            
            return responseUsers.Users.Select(a => a.UserName).ToList();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task DeleteUser(string userName)
        {
            var client = new AmazonIdentityManagementServiceClient(AWSAccessKey, AWSSecurityKey);
            var request = new DeleteUserRequest()
            {
                UserName = userName
            };

            try
            {
                var response =await client.DeleteUserAsync(request);

            }
            catch (NoSuchEntityException)
            {
                throw;
            }
        }
    }
}
