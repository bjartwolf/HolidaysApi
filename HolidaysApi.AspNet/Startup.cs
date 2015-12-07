using Owin;

namespace HolidaysApi.AspNet
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.Run (easterserver ());
		}
	}
}