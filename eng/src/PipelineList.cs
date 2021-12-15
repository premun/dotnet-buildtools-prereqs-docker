using System.Collections.Generic;

namespace Pipelines;

static class PipelineList
{
    public static readonly IEnumerable<PipelineMetadata> Pipelines = new PipelineMetadata[]
    {
        new("Alpine"),

        new("CentOS", LinuxJobTimeout: 210),

        new("Debian"),

        new("Fedora"),

        new("Mariner", RepoPath: "cbl-mariner"),

        new("NanoServer"),

        new("OpenSUSE"),

        new("Ubuntu", LinuxJobTimeout: 150, NeedsCustomBuildInit: true),

        new("WindowsServerCore"),
    };
}

record PipelineMetadata(
    string Name,
    string? RepoPath = null,
    int? LinuxJobTimeout = null,
    bool NeedsCustomBuildInit = false);
