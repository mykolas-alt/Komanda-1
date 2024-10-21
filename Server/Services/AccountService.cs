using Projektas.Shared;
using Projektas.Client.Pages;
using System.Text.Json;

namespace Projektas.Server.Services {
	public class AccountService {
		private readonly string _filepath;
		private List<AccountInfo>? AccountList=new List<AccountInfo>();

		public AccountService(string filepath) {
			_filepath=filepath;
		}

		public bool LogInToAccount(AccountInfo accountInfo) {
			string AccListSerialized;
			bool accountMached=false;
			using (StreamReader reader = new StreamReader(_filepath)) {
				AccListSerialized=reader.ReadToEnd();
			}
			AccountList=JsonSerializer.Deserialize<List<AccountInfo>>(AccListSerialized);

			foreach(AccountInfo account in AccountList) {
				if(account.Nickname==accountInfo.Nickname && account.Password==accountInfo.Password) {
					accountMached=true;
				}
			}

			return accountMached;
		}

		public void CreateAccount(AccountInfo newAccount) {
			string AccListSerialized;
			using (StreamReader reader = new StreamReader(_filepath)) {
				AccListSerialized=reader.ReadToEnd();
			}
			AccountList=JsonSerializer.Deserialize<List<AccountInfo>>(AccListSerialized);
			
			AccountList.Add(new AccountInfo(){Name=newAccount.Name,Surname=newAccount.Surname,Nickname=newAccount.Nickname,Password=newAccount.Password});
			
			AccListSerialized=JsonSerializer.Serialize(AccountList);

			using (StreamWriter writer = new StreamWriter(_filepath, append: false)) {
				writer.Write(AccListSerialized);
			}
		}

		public List<string> GetNicknames() {
			List<string> nicknames = new List<string>();

			string AccListSerialized;
			using (StreamReader reader = new StreamReader(_filepath)) {
				AccListSerialized=reader.ReadToEnd();
			}
			AccountList=JsonSerializer.Deserialize<List<AccountInfo>>(AccListSerialized);

			foreach(AccountInfo account in AccountList) {
				nicknames.Add(account.Nickname);
			}

			return nicknames;
		}
	}
}
