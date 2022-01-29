using System;

namespace BankAccountNS
{
    /// <summary>   
    /// Bank Account demo class.   
    /// </summary>   
    public class BankAccount
    {
        public double Balance { get; set; }
        public string CustomerName { get; set; }
        public bool Frozen { get; set; }
        public const string DebitAmountExceedsBalanceMessage = "Debit amount exceeds balance";
        public const string DebitAmountLessThanZeroMessage = "Debit amount less than zero";
        
        public BankAccount(string customerName, double balance)
        {
            CustomerName = customerName;
            Balance = balance;
        }

        public void Debit(double amount)
        {
            if (Frozen)
            {
                throw new Exception("Account frozen");
            }

            if (amount > Balance)
            {
                throw new ArgumentOutOfRangeException("amount", amount, DebitAmountExceedsBalanceMessage);
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, DebitAmountLessThanZeroMessage);
            }

            Balance -= amount;
        }

        public void Credit(double amount)
        {
            if (Frozen)
            {
                throw new Exception("Account frozen");
            }

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount");
            }

            Balance += amount;
        }

        public void FreezeAccount()
        {
            Frozen = true;
        }

        public void UnfreezeAccount()
        {
            Frozen = false;
        }

        public static void Main()
        {
            BankAccount ba = new BankAccount("Mr. Bryan Walton", 11.99);

            Console.WriteLine("Begining balance is ${0}", ba.Balance);
            Console.WriteLine();
            ba.Credit(5.77);
            Console.WriteLine("Crediting $5.77 to account leaving balance of ${0}", ba.Balance);
            Console.WriteLine();
            ba.Debit(11.22);
            Console.WriteLine("Debit $2000 from account after frozen");
            ba.FreezeAccount();
            try
            {
                ba.Debit(2000);
            }
            catch (Exception ex)
            {
                string message = string.Format("Unable to debit money from account because {0}", ex.Message);
                Console.WriteLine(message);
            }
            finally
            {
                ba.UnfreezeAccount();
                Console.WriteLine("Account unfrozen");
                Console.WriteLine();
            }
            Console.WriteLine("Debiting $11.22 from account leaving balance of ${0}", ba.Balance);
            Console.WriteLine("Current balance is ${0}", ba.Balance);
            Console.ReadKey();
        }

    }
}