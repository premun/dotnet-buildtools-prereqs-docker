using System.Collections.Generic;
using System.Linq;
using Sharpliner.AzureDevOps;

namespace Pipelines;

abstract class PipelineTemplate : PipelineCollection
{
    public override IEnumerable<PipelineDefinitionData<Pipeline>> Pipelines => PipelineList.Pipelines.Select(
        data => new PipelineDefinitionData<Pipeline>(
            $"eng/pipelines/dotnet-buildtools-prereqs-{data.Name.ToLower()}-pr.yml",
            CreatePipeline(data)));

    protected abstract Pipeline CreatePipeline(PipelineMetadata data);

    protected abstract string TargetFileSuffix { get; }

    protected static string GetRepoPath(PipelineMetadata data) => $"src/{data.RepoPath ?? data.Name.ToLower()}/*";

    protected static TemplateParameters GetPublishParameters(PipelineMetadata data)
    {
        var parameters = new TemplateParameters()
        {
            { "internalProjectName", "${{ variables.internalProjectName }}" },
            { "publicProjectName", "${{ variables.publicProjectName }}" },
        };

        if (data.LinuxJobTimeout.HasValue)
        {
            parameters.Add("linuxAmdBuildJobTimeout", data.LinuxJobTimeout.Value);
        }

        if (data.NeedsCustomBuildInit)
        {
            parameters.Add("customBuildInitSteps", new[]
            {
                StepTemplate("/eng/pipelines/steps/install-cross-build-prereqs.yml")
            });
        }

        return parameters;
    }
}
