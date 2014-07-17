using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iiconnect
{
	public sealed class Program
	{
		private Program()
		{

		}

		public static void Main(string[] args)
		{
			try
			{
				HitTest();
				Console.WriteLine("Ok.");
			}
			catch (Exception e)
			{
				Console.WriteLine("[ERROR] {0}", e);
			}
		}

		private static void HitTest()
		{
			DataService service = null;
			try
			{
				service = new DataService();
			}
			finally
			{
				if (service != null)
					service.Close();
			}
		}
	}
}
