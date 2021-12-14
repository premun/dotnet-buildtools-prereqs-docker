﻿using Sharpliner.AzureDevOps;

namespace Pipelines;

class NonPrPipelineTemplate : PipelineTemplate
{
    protected override string TargetFileSuffix => string.Empty;

    protected override Pipeline CreatePipeline(PipelineMetadata data) => new()
    {
        Trigger = new Trigger("main")
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
                new RepositoryResource("InternalVersionsRepo")
                {
                    Type = RepositoryType.Git,
                    Name = "internal/dotnet-versions",
                }
            }
        }
    };
}
