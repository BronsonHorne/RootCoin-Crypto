using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace recoin
{
    class Program
    {
        static void Main(string[] args)
        {
            //Generating Keys/users/wallets Using Elliptic Curve
            PrivateKey key1 = new PrivateKey();
            PublicKey userTokenWallet = key1.publicKey();

            PrivateKey key2 = new PrivateKey();
            PublicKey companyWallet = key2.publicKey();

            PrivateKey key3 = new PrivateKey();
            PublicKey userCashWallet = key3.publicKey();


            //Welcome Message
            Console.WriteLine("Welcome to Recoin!");
            Console.WriteLine("Where we Reduce, Reuse, and Renew!");

            //Creating New BlockChain
            Blockchain recoin = new Blockchain(1, 3);

            //Adding Token's To User Wallet
            recoin.HashCreationPendingTransaction(userTokenWallet);
            Console.WriteLine("\nUser's Token Balance: " + recoin.GetBalaceOfWallet(userTokenWallet));
            
            //Transferring Token's to Company Wallet for a chance of Hash Generation
            Console.WriteLine("\nTransfer Initiated");
            decimal TransferAmmount = 1;
            Transaction tx1 = new Transaction(userTokenWallet, companyWallet, TransferAmmount);

            //Signing Transfer from correct wallet to correct wallet
            tx1.SignTransaction(key1);
            recoin.addPendingTransaction(tx1);
            recoin.HashCreationPendingTransaction(companyWallet);

            //Confirms Transfer and Prints it
            Console.WriteLine("You Transfered " + TransferAmmount + " Token(s)");
            Console.WriteLine("User's Token Balance: " + recoin.GetBalaceOfWallet(userTokenWallet));
            
            //Hash Generation with 100$ of Transaction Fees
            //User earning 100 dollars of transaction fees for block mining
            recoin = new Blockchain(3, 100);
            Console.WriteLine("\nStart the Miner.");
            recoin.HashCreationPendingTransaction(userCashWallet);
            Console.WriteLine("Balance of user's Cash Wallet is $" + recoin.GetBalaceOfWallet(userCashWallet));

            string blockJSON = JsonConvert.SerializeObject(recoin, Formatting.Indented);

            //Checks if Chain is Valid to prevent tampering
            if(recoin.IsChainValid())
            {
                Console.WriteLine("Blockchain is Valid!");
            }
            else
            {
                Console.WriteLine("Blockchain is NOT Valid!");
            }
        }
    }
}
