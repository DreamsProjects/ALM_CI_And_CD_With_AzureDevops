using ALM.Models;
using ALM.Repository;
using ALM.UnitTestData;
using System;
using System.Linq;
using Xunit;

namespace xUnitTestBank
{
    public class UnitTest1 
    {
        [Theory]
        [InlineData(1, 2000, "6500")]
        public void Deposit_CompareString(int number, int amount, string expected) //Verifiera rätt summa efter insättning
        {
            TransactionData generateTransaction = new TransactionData();

            var stringResult = generateTransaction.DepositMoney(number, amount);
            
            Assert.Equal(expected, stringResult);
        }


        [Theory]
        [InlineData(1, 2000, "2500")]
        [InlineData(1, 4600, "wrongAnswer")] //Finns bara 4500 på kontot
        public void Withdraw_CompareString(int number, int amount, string expected) //Verifiera rätt summa efter uttag
        {
            //Arrange
            TransactionData generateTransaction = new TransactionData();

            var stringResult = generateTransaction.WithdrawAmount(number, amount);

            //Assert
            Assert.Equal(expected, stringResult);
        }

        [Theory]
        [InlineData("Godkant", 1, 2000)]
        [InlineData("Godkant", 1, 4500)]
        [InlineData("Nekad", 1, 4502)]
        public void Confirm_ReturnString(string expect, int number, int amount) //Verifiera för stort uttag
        {
            TransactionData confirmTransacion = new TransactionData();

            var compareExpectedValue = confirmTransacion.Verify(number, amount);

            Assert.Equal(expect, compareExpectedValue);
        }


        [Theory]
        [InlineData(49, 100, "49", 0, 149)]
        [InlineData(200, 99, "1", 199, 100)] // 
        public void Transfer_Should_Succeed(decimal fromBalance, decimal toBalance, string sum, decimal expectedFromResult, decimal expectedToResult)
        {
            BankRepository _bank = new BankRepository();

            //arrange
            Account fromAccount = new Account
            {
                AccountId = 1,
                PersonId = 1001,
                Money = fromBalance
            };

            Account toAccount = new Account
            {
                AccountId = 2,
                PersonId = 1002,
                Money = toBalance
            };

            _bank.Accounts.Add(fromAccount);
            _bank.Accounts.Add(toAccount);

            var fromCustomer = new Customer
            {
                PersonId = 1001,
                Name = "A",
                LastName = "A",
                Account = _bank.Accounts.Where(x => x.PersonId == 1001).ToList()
            };

            var toCustomer = new Customer
            {
                PersonId = 1002,
                Name = "B",
                LastName = "B",
                Account = _bank.Accounts.Where(x => x.PersonId == 1002).ToList()
            };

            _bank.Customers.Add(fromCustomer);
            _bank.Customers.Add(toCustomer);

            // act
            var result = _bank.Transfer(1, 2, sum);

            // assert            
            Assert.Equal("OK", result);
            Assert.Equal(expectedFromResult, fromAccount.Money);
            Assert.Equal(expectedToResult, toAccount.Money);
        }

        [Theory]
        [InlineData(49, "50", "OVERDRAW ERROR")]
        [InlineData(50, "100", "OVERDRAW ERROR")] 
        public void Transfer_Should_Fail(decimal fromBalance, string sum, string expectedResult)
        {
            BankRepository _bank = new BankRepository();

            //arrange
            Account fromAccount = new Account
            {
                AccountId = 1,
                PersonId = 1001,
                Money = fromBalance
            };

            Account toAccount = new Account
            {
                AccountId = 2,
                PersonId = 1002,
                Money = fromBalance
            };

            _bank.Accounts.Add(fromAccount);
            _bank.Accounts.Add(toAccount);

            var fromCustomer = new Customer
            {
                PersonId = 1001,
                Name = "A",
                LastName = "A",
                Account = _bank.Accounts.Where(x => x.PersonId == 1001).ToList()
            };

            var toCustomer = new Customer
            {
                PersonId = 1002,
                Name = "B",
                LastName = "B",
                Account = _bank.Accounts.Where(x => x.PersonId == 1002).ToList()
            };
            
            _bank.Customers.Add(fromCustomer);
            _bank.Customers.Add(toCustomer);

            // act
            var result = _bank.Transfer(1, 2, sum);

            // assert            
            Assert.Equal(expectedResult, result);
        }
    }
}
