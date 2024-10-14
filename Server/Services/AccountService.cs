using Projektas.Shared;
using Projektas.Client.Pages;
using System.Text.Json;

namespace Projektas.Server.Services {
	public class AccountService {
		public string Test {get;set;}="testing";
		private List<AccountInfo> AccountList=new List<AccountInfo>();

		public string GetTestServ() {
			/*AccountList.Add(new AccountInfo(){Name="Artiom"});
			AccountList.Add(new AccountInfo(){Name="Tomas"});
			
			var serialized=JsonSerializer.Serialize(AccountList);

			using (StreamWriter writer = new StreamWriter("AccountInfo.txt")) {
				writer.Write(serialized);
			}*/

			return Test;
		}
	}
}
