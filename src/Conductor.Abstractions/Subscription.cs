using System;
using System.Collections.Generic;

namespace Conductor.Abstractions
{
    public record Subscription(string ChannelName, string SourceRepositoryUrl, string TargetRepositoryUrl, string TargetBranchName, UpdateFrequency UpdateFrequency, IEnumerable<string> Policies)
    {
        public Guid Id { get; init; }
        public IEnumerable<Dependency> Assets { get; set; } // TODO: implement
        public bool HasFailureNotificationTags { get; set; } // TODO: implement
        public bool IsNotBatched { get; set; } // TODO: implement

        public bool IsTriggered(BuildInfo build) => false; // TODO: implement
        public bool IsDesiredQuality(Repository repository) => true; // TODO: implement
    }
}
