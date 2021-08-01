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

        public bool IsTriggered(BuildInfo build)
        {
	        // TODO: implement
	        return false;
        }

        public bool IsDesiredQuality(Repository repository)
        {
            // perform build, static analysis, etc.

	        // TODO: implement
	        return true;
        }
    }
}
