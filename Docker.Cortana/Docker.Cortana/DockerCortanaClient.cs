using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.X509;
using Docker.DotNet.Models;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Diagnostics;

namespace Docker.Cortana
{
    public class DockerCortanaClient : IDockerCortanaClient
    {

        private DockerClient client;
        private string _host;

        #region Password
        //TODO: Find a better place for this
        private string password = "DockerR0cks!"; 
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">Format: tcp://dockercon-web.westus.cloudapp.azure.com:2376</param>
        public DockerCortanaClient(string host)
        {
            _host = host;
            SetupDockerClient();
        }

        private void SetupDockerClient()
        {
            //string pfxFilePath = Environment.CurrentDirectory + @"\key.pfx";
            //Debug.WriteLine("Path=" + pfxFilePath);
            //byte[] file = File.ReadAllBytes(pfxFilePath); 
            //var credentials = new CertificateCredentials(new X509Certificate2(file, password));
            //client = new DockerClientConfiguration(new Uri(_host), credentials).CreateClient();
            client = new DockerClientConfiguration(new Uri(_host)).CreateClient();

        }


        public async Task<CreateContainerResponse> DockerRun(string imageName)
        {
            var runParams = new DotNet.Models.CreateContainerParameters();
            //runParams.Config.Tty = true;                    

           return await client.Containers.CreateContainerAsync(runParams);

        }

        public async Task<IList<ContainerListResponse>> DockerGetContainers()
        {
            var containerParams = new ListContainersParameters();
            containerParams.All = true;
            
            var response = await client.Containers.ListContainersAsync(containerParams);
            return response; 
        }


        public async Task<CreateContainerResponse> DockerRun(string imageName, int hostPort, int containerPort)
        {
            var runParams = new DotNet.Models.CreateContainerParameters();

            runParams.Config.AttachStderr = true;
            runParams.Config.AttachStdout = true;
            //runParams.Config.ExposedPorts = 

                var ports = new Dictionary<string, object>();
             string portString = @"{
                                'HostIp': '0.0.0.0',
                                'HostPort': '49153'
                            }";


            ports.Add(hostPort.ToString() + "/tcp", portString); 



            //"80/tcp": [
            //                {
            //                    "HostIp": "0.0.0.0",
            //                    "HostPort": "49153"
            //                }

            runParams.Config.Tty = true;
            return await client.Containers.CreateContainerAsync(runParams);

        }
        /// <summary>
        /// CancellationTokenSource cancellation = new CancellationTokenSource();
        /// Stream stream = ShowLogs(id, cancellation)
        /// </summary>
        /// <param name="id">Container ID</param>
        /// <param name="cancellationToken">CancellationTokenSource</param>
        /// <returns>Stream</returns>
        public async Task<Stream> ShowLogs(string id, System.Threading.CancellationToken cancellationToken)
        {
            var logParams = new GetContainerLogsParameters();
            logParams.Follow = true;

            var Stream = await client.Containers.GetContainerLogsAsync(id, logParams, cancellationToken);
            return Stream; 
        }

        public async Task<SystemInfoResponse> DockerInfo()
        {
            SystemInfoResponse response = new SystemInfoResponse();
            response = await client.Miscellaneous.GetSystemInfoAsync();
            return response; 
        }


    }
}
