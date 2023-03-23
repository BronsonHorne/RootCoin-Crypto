using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace EnergyFi
{
    class Program
    {
        static void Main(string[] args)
        {
            PrivateKey key1 = new PrivateKey();
            PublicKey userTokenWallet = key1.publicKey();

            PrivateKey key2 = new PrivateKey();
            PublicKey companyWallet = key2.publicKey();

            PrivateKey key3 = new PrivateKey();
            PublicKey userCashWallet = key3.publicKey();



            Console.WriteLine("Welcome to EnergyFi!");

            Blockchain rootcoin = new Blockchain(1, 3);
            rootcoin.HashCreationPendingTransaction(userTokenWallet);
            Console.WriteLine("\nUser's Token Balance: " + rootcoin.GetBalaceOfWallet(userTokenWallet));

            Console.WriteLine("\nTransfer Initiated");
            decimal TransferAmmount = 1;
            Transaction tx1 = new Transaction(userTokenWallet, companyWallet, TransferAmmount);
            tx1.SignTransaction(key1);
            rootcoin.addPendingTransaction(tx1);
            //rootcoin.HashCreationPendingTransaction(companyWallet);
            
            rootcoin.HashCreationPendingTransaction(companyWallet);

            Console.WriteLine("You Transfered " + TransferAmmount + " Token(s)");
            Console.WriteLine("User's Token Balance: " + rootcoin.GetBalaceOfWallet(userTokenWallet));
            
            rootcoin = new Blockchain(3, 100);
            Console.WriteLine("\nStart the Miner.");
            rootcoin.HashCreationPendingTransaction(userCashWallet);
            Console.WriteLine("Balance of user's Cash Wallet is $" + rootcoin.GetBalaceOfWallet(userCashWallet));

            string blockJSON = JsonConvert.SerializeObject(rootcoin, Formatting.Indented);
            //Console.WriteLine(blockJSON);

            //rootcoin.GetLastestBlock().PreviousHash = "12345";

            if(rootcoin.IsChainValid())
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
