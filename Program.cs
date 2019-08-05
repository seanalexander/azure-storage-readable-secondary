using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace blob_quickstart
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("Azure Blob Storage - .NET quickstart sample\n");

            // Run the examples asynchronously, wait for the results before proceeding
            ProcessAsync().GetAwaiter().GetResult();

            

            //Console.WriteLine("Press any key to exit the sample application.");
            //Console.ReadLine();
        }

        private static async Task ProcessAsync()
        {
            // Retrieve the connection string for use with the application. The storage 
            // connection string is stored in an environment variable on the machine 
            // running the application called CONNECT_STR. If the 
            // environment variable is created after the application is launched in a 
            // console or with Visual Studio, the shell or application needs to be closed
            // and reloaded to take the environment variable into account.
            string storageConnectionString = Environment.GetEnvironmentVariable("CONNECT_STR");

            // Check whether the connection string can be parsed.
            CloudStorageAccount storageAccount;
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                // If the connection string is valid, proceed with operations against Blob
                // storage here.
                // ADD OTHER OPERATIONS HERE
            }
            else
            {
                // Otherwise, let the user know that they need to define the environment variable.
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add an environment variable named 'CONNECT_STR' with your storage " +
                    "connection string as a value.");
                Console.WriteLine("Press any key to exit the application.");
                Console.ReadLine();
            }

            // Create the CloudBlobClient that represents the 
            // Blob storage endpoint for the storage account.
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();



            BlobRequestOptions blobRequestOptions = new BlobRequestOptions()
            {
                //LocationMode = Microsoft.WindowsAzure.Storage.Blob.LocationMode.SecondaryOnly,
                LocationMode = Microsoft.Azure.Storage.RetryPolicies.LocationMode.SecondaryOnly
                //LocationMode = Microsoft.Azure.Storage.Blob.lo
            };

            // Create a container called 'quickstartblobs' and 
            // append a GUID value to it to make the name unique.
            CloudBlobContainer cloudBlobContainer = 
                cloudBlobClient.GetContainerReference("replicateme");

            string localPath = @"C:\Users\self_dmsw2ui\dev\azure\storage\ReadFileFromSecondary\blob-quickstart\tmp\";
            string localFileName = "TotallyReadMeFromSecodary.txt";
            string sourceFile = Path.Combine(localPath, localFileName);

            Console.WriteLine("Temp file = {0}", sourceFile);
            Console.WriteLine("Uploading to Blob storage as blob '{0}'", localFileName);

            // Get a reference to the blob address, then upload the file to the blob.
            // Use the value of localFileName for the blob name.

            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(localFileName);

            string destinationFile = sourceFile.Replace(".txt", "_PRIMARY.txt");
            Console.WriteLine("Downloading blob to {0}", destinationFile);
            await cloudBlockBlob.DownloadToFileAsync(destinationFile, FileMode.Create);

            //AccessCondition streamAccessCondition = null;
            //OperationContext operationContext = null;
            
            destinationFile = sourceFile.Replace(".txt", "_SECONDARY.txt");
            Console.WriteLine("Downloading blob to {0}", destinationFile);
            await cloudBlockBlob.DownloadToFileAsync(destinationFile,FileMode.Create,null,blobRequestOptions,null);
            
            var blob = cloudBlobContainer.GetBlobReference(destinationFile);


        }
    }
}