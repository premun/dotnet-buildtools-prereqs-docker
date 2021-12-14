using Sharpliner;
using Sharpliner.AzureDevOps;
using Sharpliner.AzureDevOps.ConditionedExpressions;

namespace Pipelines;

class CommonVariables : VariableTemplateDefinition
{
    public override TargetPathType TargetPathType => TargetPathType.RelativeToGitRoot;
    public override string TargetFile => "eng/pipelines/variables/common.yml";

    public override ConditionedList<VariableBase> Definition => new()
    {
        VariableTemplate("../../common/templates/variables/dotnet/build-test-publish.yml"),

        Variable("manifest", "manifest.json"),
        Variable("dotnetVersion", "*"),
        Variable("osVariant", ""),
        Variable("publishReadme", false),
        Variable("publicGitRepoUri", "https://github.com/dotnet/dotnet-buildtools-prereqs-docker"),
        Variable("manifestVariables", "--var UniqueId=$(sourceBuildId)"),
        Variable("publicSourceBranch", "main"),
        Variable("ingestKustoImageInfo", false),

        If.Equal(variables["System.TeamProject"], "'internal'")
            .Variable("build.imageBuilderDockerRunExtraOptions",
                      "-e DOCKER_REPO=$(acr.server)/$(stagingRepoPrefix)dotnet-buildtools/prereqs")
    };
}
