using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace AzureStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Configuring Azure blob storage........");

            string connectionString = "your connection string";

            Program p = new Program();
            p.Container(connectionString:connectionString);
            Console.ReadKey();
        }
        public  async void Container(string connectionString)
        {
            BlobServiceClient blob = new BlobServiceClient(connectionString);
            string containerName = "blobcontainer"+ Guid.NewGuid().ToString();
            BlobContainerClient containerClient = await blob.CreateBlobContainerAsync(containerName);

            System.Console.WriteLine("Configuring Container a success");
            System.Console.WriteLine("Attempting to upload data...");
            Upload(containerClient);
        }
        public async void Upload(BlobContainerClient containerClient)
        {
            string localPath = "./data";
            string fileName = "" + Guid.NewGuid().ToString() + ".txt";
            string path = Path.Combine(localPath,fileName);

            await File.WriteAllTextAsync(path,"Oliver Kipkemei writes C# and blockchain code");

            BlobClient blob = containerClient.GetBlobClient(fileName);

            System.Console.WriteLine("Uploading data to the blob client {0}.......",blob.Uri);

            using FileStream uploadFileStream = File.OpenRead(path);
            await blob.UploadAsync(uploadFileStream,true);
            uploadFileStream.Close();
            System.Console.WriteLine("Data uploaded successfully");
            System.Console.WriteLine("Attempt to dowload data");
            Download(path,blob);
        }
        public async void Download(string path, BlobClient blob)
        {
            string downloadPath = path.Replace(".txt","DOWNLOADED.txt");
            System.Console.WriteLine("DOwnloading data from blob");

            BlobDownloadInfo downloadInfo = await blob.DownloadAsync();
            
            using (FileStream downloadFileStream = File.OpenWrite(downloadPath))
            {
                await downloadInfo.Content.CopyToAsync(downloadFileStream);
                downloadFileStream.Close();
            }
            System.Console.WriteLine("Download a success");
        }
    }
}
