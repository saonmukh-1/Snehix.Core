using Amazon;
using Amazon.IdentityManagement;
using Amazon.IdentityManagement.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Snehix.Core.API.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AmazonS3Service
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
        /// <param name="bucketName"></param>
        /// <returns></returns>
        public async Task CreateBucketToS3(string bucketName)
        {
            using (var client = new AmazonS3Client(AWSAccessKey, AWSSecurityKey, RegionEndpoint.USEast1))
            {
                    var uploadRequest = new PutBucketRequest
                    { 
                        BucketName = bucketName,
                        CannedACL = S3CannedACL.PublicRead,
                        BucketRegion= S3Region.APS3
                    };
                await client.PutBucketAsync(uploadRequest);
                await CreateFolder(bucketName, "newfolder"); 
                
            }
        }

        public async Task CreateFolder(string bucketName, string folderName)
        {
            var client = new AmazonS3Client(AWSAccessKey, AWSSecurityKey, RegionEndpoint.USEast1);
            var folderKey = folderName + "/"; //end the folder name with "/"
            var request = new Amazon.S3.Model.PutObjectRequest()
            {
                BucketName = bucketName,
                Key = folderKey // <-- in S3 key represents a path  
            }; 
            //request.StorageClass = S3StorageClass.Standard;
            //request.ServerSideEncryptionMethod = ServerSideEncryptionMethod.None;
            //request.CannedACL = S3CannedACL.BucketOwnerFullControl;           

            var response = await client.PutObjectAsync(request);
        }

    }

}

