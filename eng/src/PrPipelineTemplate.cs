using Sharpliner.AzureDevOps;

namespace Pipelines;

class PrPipelineTemplate : PipelineTemplate
{
    protected override string TargetFileSuffix => "-pr";

    protected override Pipeline CreatePipeline(PipelineMetadata data) => new()
    {
        Pr = new PrTrigger("main")
        {
            Paths = new()
            {
                Include = { $"src/{data.RepoPath ?? data.Name.ToLower()}/*" }
            }
        },

        Resources = new Resources()
        {
            Repositories =
            {
                new RepositoryResource("PublicVersionsRepo")
                {
                    Type = RepositoryType.GitHub,
                    Endpoint = "dotnet",
                    Name = "dotnet/versions",
                }
            }
        },

        Variables =
        {
            VariableTemplate("variables/common.yml"),
            Variable("imageBuilder.pathArgs", $"--path 'src/{data.RepoPath ?? data.Name.ToLower()}/*'"),
        },

        Stages =
        {
            StageTemplate("../common/templates/stages/dotnet/build-test-publish-repo.yml", GetPublishParameters(data))
        }
    };
}
