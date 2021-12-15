using System.Collections.Generic;
using System.Linq;
using Sharpliner.AzureDevOps;

namespace Pipelines;

abstract class PipelineTemplate : PipelineCollection
{
    protected virtual IEnumerable<PipelineMetadata> PipelineDefinitions => PipelineList.Pipelines;

    public override IEnumerable<PipelineDefinitionData<Pipeline>> Pipelines => PipelineDefinitions.Select(
        data => new PipelineDefinitionData<Pipeline>(GetYamlPath(data), AddCommonParts(CreatePipeline(data), data)));

    protected abstract Pipeline CreatePipeline(PipelineMetadata data);

    protected abstract string TargetFileSuffix { get; }

    protected string GetYamlPath(PipelineMetadata data) => $"eng/pipelines/dotnet-buildtools-prereqs-{data.Name.ToLower()}{TargetFileSuffix}.yml";

    protected virtual string GetDockerPath(PipelineMetadata data) => $"src/{data.RepoPath ?? data.Name.ToLower()}/*";

    protected Pipeline AddCommonParts(Pipeline pipeline, PipelineMetadata data) => pipeline with
    {
        Variables = new()
        {
            VariableTemplate("variables/common.yml"),
            Variable("imageBuilder.pathArgs", $"--path '{GetDockerPath(data)}'"),
        },

        Stages = new()
        {
            StageTemplate("../common/templates/stages/dotnet/build-test-publish-repo.yml", GetPublishParameters(data))
        }
    };
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
