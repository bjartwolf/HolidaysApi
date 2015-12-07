using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Host.SystemWeb;
using HolidaysApi.OwinApp;

namespace HolidaysApi.AspNet
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.Run(new EasterServer());
		}
	}
}