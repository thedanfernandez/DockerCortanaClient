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
    public class DockerCortanaClient 
    {

        private DockerClient client;
        private string _host;

        /// <summary>
        /// Only works on insecure Docker for now
        /// </summary>
        /// <param name="host">Format: tcp://vm.cloudapp.net:2376</param>
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

        /// <summary>
        /// imageName must be on server already otherwise expect a 404
        /// </summary>
        /// <param name="imageName">name of the image, ex: redis</param>
        /// <returns></returns>
        public async Task<string> DockerRun(string imageName)
        {
            //            docker create
            // if miss - 404 on API
            //    docker pull imagename
            //docker create
            //docker start


            var runParams = new DotNet.Models.CreateContainerParameters();
            
            Config y = new Config();
            runParams.Config = y;

            runParams.Config.Tty = true;
            runParams.Config.AttachStderr = true;
            runParams.Config.AttachStdout = true;
            
            
            runParams.Config.Image = imageName;
            

            var result =  await client.Containers.CreateContainerAsync(runParams);
            var hostconfig = new HostConfig();

            await client.Containers.StartContainerAsync(result.Id, hostconfig);
            return result.Id; 

        }

        /// <summary>
        /// Returns a list of running containers, ex: docker ps
        /// </summary>
        /// <returns></returns>
        public async Task<IList<ContainerListResponse>> DockerGetContainers()
        {
            var containerParams = new ListContainersParameters();
            containerParams.All = false;
            
            var response = await client.Containers.ListContainersAsync(containerParams);
            return response; 
        }

        /// <summary>
        /// Returns information about containers for a given host with option to show stopped containers, ex:docker ps -a
        /// </summary>
        /// <param name="all">if true, returns stopped containers</param>
        /// <returns></returns>
        public async Task<IList<ContainerListResponse>> DockerGetContainers(bool all)
        {
            var containerParams = new ListContainersParameters();
            containerParams.All = all;

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
        /// Returns a log stream for Standard error and standard output
        /// </summary>
        /// <param name="id">Container ID</param>
        /// <param name="cancellationToken">CancellationTokenSource</param>
        /// <returns>Stream</returns>
        public async Task<Stream> ShowLogs(string id, System.Threading.CancellationToken cancellationToken)
        {
            var logParams = new GetContainerLogsParameters();
            //logParams.Follow = true;
            logParams.Stderr = true;
            logParams.Stdout = true; 

            var Stream = await client.Containers.GetContainerLogsAsync(id, logParams, cancellationToken);
            return Stream; 
        }

        /// <summary>
        /// Used for diagnostic info, ex: docker info
        /// </summary>
        /// <returns>system info</returns>
        public async Task<SystemInfoResponse> DockerInfo()
        {
            SystemInfoResponse response = new SystemInfoResponse();
            response = await client.Miscellaneous.GetSystemInfoAsync();
            return response; 
        }


    }
}
