using ALM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ALM.UnitTestData
{
    public class TransactionData
    {
        public string WithdrawAmount(int number, int amount) //Inte kunna ta ut mer än det som finns
        {
            var listOfAccounts = GetAccounts().Where(x => x.AccountId == number).FirstOrDefault();

            if (listOfAccounts.Money >= amount) //Om beloppet finns
            {
                listOfAccounts.Money = listOfAccounts.Money - amount;

                return listOfAccounts.Money.ToString();
            }

            return "wrongAnswer";
        }

        public string DepositMoney(int number, int amount) //Lägger in pengar
        {
            var listOfAccounts = GetAccounts().Where(x => x.AccountId == number).FirstOrDefault();

            if (amount > 0)
            {
                listOfAccounts.Money = listOfAccounts.Money + amount;

                return listOfAccounts.Money.ToString();
            }

            return "wrongAnswer";
        }

        public List<Account> GetAccounts()
        {
            var listOfAccounts = new List<Account>();

            var account = new Account
            {
                AccountId = 1,
                Name = "Emma",
                LastName = "Davidsson",
                Money = 4500
            };

            listOfAccounts.Add(account);

            return listOfAccounts;
        }


        public string Verify(int number, int amount)
        {
            var listOfAccounts = GetAccounts().Where(x => x.AccountId == number).FirstOrDefault();

            if (listOfAccounts.Money >= amount)
            {
                return "Godkant";
            }

            else
            {
                return "Nekad";
            }
        }
    }
}
