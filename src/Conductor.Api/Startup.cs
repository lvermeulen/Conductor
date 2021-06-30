using Conductor.Channels.Handlers;
using Conductor.Subscriptions.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MediatR;
using Conductor.Api.Controllers;
using Conductor.Api.Extensions;

namespace Conductor.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddConductor()
				.AddMediatR(typeof(AddChannelHandler), typeof(AddSubscriptionHandler))
				.AddTransient<ChannelsController>()
				.AddTransient<SubscriptionsController>();

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Conductor.Api", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app
					.UseDeveloperExceptionPage()
					.UseSwagger()
					.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Conductor.Api v1"));
			}

			app
				.UseHttpsRedirection()
				.UseRouting()
				.UseAuthorization()
				.UseEndpoints(endpoints =>
				{
					endpoints.MapControllers();
				});
		}
	}
}
