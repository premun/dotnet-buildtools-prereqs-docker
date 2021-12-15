using System.Collections.Generic;
using Sharpliner.AzureDevOps;

namespace Pipelines;

class EngPipelineTemplate : PrPipelineTemplate
{
    protected override IEnumerable<PipelineMetadata> PipelineDefinitions => new[]
    {
        new PipelineMetadata(
            Name: "eng",
            RepoPath: "*",
            LinuxJobTimeout: 210,
            NeedsCustomBuildInit: true)
    };

    protected override string TargetFileSuffix => string.Empty;
    
    protected override string GetDockerPath(PipelineMetadata data) => "*";

    protected override Pipeline CreatePipeline(PipelineMetadata data) => base.CreatePipeline(data) with
    {
        Pr = new PrTrigger("main")
        {
            Paths = new()
            {
                Exclude = { "src/*", "README.md" }
            }
        }
    };
}
