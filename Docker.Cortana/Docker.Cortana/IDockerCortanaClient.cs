using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace Docker.Cortana
{
    public interface IDockerCortanaClient
    {
        Task<IList<ContainerListResponse>> DockerGetContainers();
        Task<SystemInfoResponse> DockerInfo();
        Task<CreateContainerResponse> DockerRun(string imageName);
        Task<CreateContainerResponse> DockerRun(string imageName, int hostPort, int containerPort);
        Task<Stream> ShowLogs(string id, CancellationToken cancellationToken);
    }
}