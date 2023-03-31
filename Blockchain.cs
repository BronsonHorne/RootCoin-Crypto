using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using EllipticCurve;

namespace recoin
{
    //Creates Blockchain Class
    class Blockchain
{
        public List<Block> Chain { get; set; }

        public int Difficulty { get; set; }

        public List<Transaction> pendingTransactions { get; set; }

        public decimal MiningReward { get; set; }

        //Creates Blockchain with a Genesis Block along with Transactions
        public Blockchain(int difficulty, decimal miningReward)
        {
            this.Chain = new List<Block>();
            this.Chain.Add(CreateGenesisBlock());
            this.Difficulty = difficulty;
            this.MiningReward = miningReward;
            this.pendingTransactions = new List<Transaction>();
        }

        //Create Genesis Block for the Block Chain
        public Block CreateGenesisBlock()
        {
            return new Block(0, DateTime.Now.ToString("yyyMMddHHmmssffff"), new List<Transaction>());
        }

        //Gets Latest Block in Blockchain for Validation and new block creation
        public Block GetLastestBlock()
        {
            return this.Chain.Last();
        }

        //Adds A Block to the Blockchain
        public void AddBlock(Block newBlock)
        {
            newBlock.PreviousHash = this.GetLastestBlock().Hash;
            newBlock.Hash = newBlock.CalculateHash();
            this.Chain.Add(newBlock);
        }

        //Checks to see if Tokens are available in wallet for transactions
        public void addPendingTransaction(Transaction transaction)
        {
            if (transaction.FromAddress is null || transaction.ToAddress is null)
            {
                throw new Exception("Transactions must include a to and from address.");
            }
            if(transaction.Amount > this.GetBalaceOfWallet(transaction.FromAddress))
            {
                throw new Exception("There must be sufficient money in the wallet!");
            }
            
            if (transaction.IsValid() == false)
            {
                throw new Exception("Cannot add an invalid transaction to a block.");
            }

            this.pendingTransactions.Add(transaction);
        }

        //Displays Token Balance
        public decimal GetBalaceOfWallet(PublicKey address , decimal addition = 0)
        {
            decimal balance = addition;

            string addressDER = BitConverter.ToString(address.toDer()).Replace("-", "");

            foreach (Block block in this.Chain)
            {
                foreach (Transaction transaction in block.Transactions)
                {
                    if (!(transaction.FromAddress is null))
                    {


                        string fromDER = BitConverter.ToString(transaction.FromAddress.toDer()).Replace("-", "");
                        if (fromDER == addressDER)
                        {
                            balance -= transaction.Amount;
                        }
                    }
                    
                    string toDER = BitConverter.ToString(transaction.ToAddress.toDer()).Replace("-", "");
                    if (toDER == addressDER)
                    {
                        balance += transaction.Amount;
                    }

                }
            }
            return balance;
        }

        //Generates Hash for the Blockchain and gives the miner the transaction fees
        public void HashCreationPendingTransaction(PublicKey miningRewardWallet)
        {
            Transaction rewardTx = new Transaction(null, miningRewardWallet, MiningReward);
            this.pendingTransactions.Add(rewardTx);

            Block newBlock = new Block(GetLastestBlock().Index + 1, DateTime.Now.ToString("yyyMMddHHmmssffff"), this.pendingTransactions, GetLastestBlock().Hash);
            newBlock.HashCreation(this.Difficulty);
            this.Chain.Add(newBlock);
            this.pendingTransactions = new List<Transaction>();
        }

        public bool IsChainValid()
        {
            for (int i = 1; i < this.Chain.Count; i++)
            {
                Block currentBlock = this.Chain[i];
                Block previousBlock = this.Chain[i - 1];

                //Check if the current block hash is same as calculated hash
                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }

            return true;
        }
        
    }
}
