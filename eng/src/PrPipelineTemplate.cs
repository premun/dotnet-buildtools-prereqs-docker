﻿using Sharpliner.AzureDevOps;

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
                Include = { $"src/{GetRepoPath(data)}/*" }
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
    };
}
