using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conductor.Channels;

namespace Conductor.Core
{
	public class ConductorService
	{
		public ConcurrentDictionary<string, Channel> Channels { get; } = new ConcurrentDictionary<string, Channel>();
		public IList<ExpressionDetailFile> ExpressionDetailFiles { get; }
		public IList<ExpressionFile> ExpressionFiles { get; }

		internal ConductorService(IList<ExpressionDetailFile> expressionDetailFiles, IList<ExpressionFile> expressionFiles)
		{
			ExpressionDetailFiles = expressionDetailFiles;
			ExpressionFiles = expressionFiles;
		}

		public Task<Channel> AddChannelAsync(string name, ClassificationType classificationType, string repositoryUrl, string branchName)
		{
			var channel = new Channel(name, classificationType, repositoryUrl, branchName);
			var newChannel = Channels.AddOrUpdate(name, channelName => channel, (channelName, oldChannel) => channel);

			return Task.FromResult(newChannel);
		}

		public async Task RemoveChannelAsync(string name)
		{
			var channel = await FindChannelByNameAsync(name);
			Channels.TryRemove(channel.Name, out var _);
		}

		public Task<Channel> FindChannelByNameAsync(string channelName) => Task.FromResult(Channels.FirstOrDefault(x => x.Value.Name.Equals(channelName, StringComparison.InvariantCultureIgnoreCase)).Value);
	}
}
