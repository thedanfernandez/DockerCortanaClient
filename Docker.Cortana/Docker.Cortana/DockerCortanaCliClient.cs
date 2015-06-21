using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using System.Diagnostics;

namespace Docker.Cortana
{
    public class DockerCortanaCliClient 
    {


        public DockerCortanaCliClient()
        {

        }

        public Task<IList<ContainerListResponse>> DockerGetContainers()
        {
            throw new NotImplementedException();
        }

        public string DockerInfo()
        {
            Process p = new Process();

            var start = new ProcessStartInfo("cmd.exe", "docker --tls info");
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            p.StartInfo = start;

            //Start!
            p.Start();

            //Get output written to standard out as a string
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output; 

        }

        public Task<CreateContainerResponse> DockerRun(string imageName)
        {
            throw new NotImplementedException();
        }

        public Task<CreateContainerResponse> DockerRun(string imageName, int hostPort, int containerPort)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> ShowLogs(string id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
