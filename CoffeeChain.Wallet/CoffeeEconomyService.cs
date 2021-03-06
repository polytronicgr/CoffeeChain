﻿using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using CoffeeChain.Wallet.FunctionOutputs;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace CoffeeChain.Wallet
{
    public class CoffeeEconomyService
    {
        private static HexBigInteger DefaultGasToUse = new HexBigInteger(140000);
        private static HexBigInteger DefaultGasValue = new HexBigInteger(144000000000000000);

        private readonly Account _account;
        private readonly Web3 _web3;
        private readonly Contract _contract;

        public CoffeeEconomyService(Account account, Web3 web3, string address)
        {
            _account = account;
            _web3 = web3;

            var abi = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "abi.json"));
            _contract = web3.Eth.GetContract(abi, address);
        }

        public async Task<string> AddAuthorizedExchangeWallet(string address)
        {
            var addAuthorizedExchangeWallet = _contract.GetFunction("addAuthorizedExchangeWallet");
            return await addAuthorizedExchangeWallet.SendTransactionAsync(_account.Address, DefaultGasToUse, DefaultGasValue, null, address);
        }

        public async Task<string> AddCustomer(string address, string name, string department, string telephone, string email)
        {
            var addCustomer = _contract.GetFunction("addCustomer");
            return await addCustomer.SendTransactionAsync(_account.Address, DefaultGasToUse, DefaultGasValue, null, address,
                name, department, telephone, email);
        }

        public async Task<string> AddCoffeemaker(string target, string name, string locDescriptive, string locDepartment,
            string locLatitude, string locLongitude, int infoMachineType, string infoDescription)
        {
            var addCoffeemaker = _contract.GetFunction("addCoffeemaker");
            return await addCoffeemaker.SendTransactionAsync(_account.Address, DefaultGasToUse, DefaultGasValue, null, target,
                name, locDescriptive, locDepartment, locLatitude, locLongitude, infoMachineType, infoDescription);
        }

        public async Task<string> AddCoffeemakerPogram(string target, string name, int price)
        {
            var addcoffeemakerprogramm = _contract.GetFunction("addCoffeeMakerProgram");
            return await addcoffeemakerprogramm.SendTransactionAsync(_account.Address, DefaultGasToUse, DefaultGasValue,
                null, target, name, price);
        }

        public async Task<string> BuyTokens(string forAddress, BigInteger amount)
        {
            var buyTokens = _contract.GetFunction("buyTokens");
            return await buyTokens.SendTransactionAsync(_account.Address, DefaultGasToUse, DefaultGasValue, new HexBigInteger(amount), forAddress);
        }

        public async Task<string> SellTokens(string seller, int tokens)
        {
            var selltokens = _contract.GetFunction("sellTokens");
            return await selltokens.SendTransactionAsync(_account.Address, DefaultGasToUse, DefaultGasValue, null, seller, tokens);
        }

        public async Task<string> TransfareTokens(string receiver, int tokens)
        {
            var transtokens = _contract.GetFunction("transferTokens");
            return await transtokens.SendTransactionAsync(_account.Address, DefaultGasToUse, DefaultGasValue, null, receiver, tokens);
        }

        public async Task<string> BuyCoffee(string coffeeMaker, int program, int amount)
        {
            var BuyCoffee = _contract.GetFunction("buyCoffee");
            return await BuyCoffee.SendTransactionAsync(_account.Address, DefaultGasToUse, DefaultGasValue, null, coffeeMaker, program, amount);
        }

        public async Task<int> GetTokens(string address)
        {
            return await _contract.GetFunction("getTokens").CallAsync<int>(address);
        }

        public async Task<Customer> GetCustomerData(string wallet)
        {
            return await _contract.GetFunction("getCustomerData").CallDeserializingToObjectAsync<Customer>(wallet);
        }

        public async Task<CoffeeMaker> GetCoffeeMakerData(string wallet)
        {
            return await _contract.GetFunction("getCoffeeMakerData").CallDeserializingToObjectAsync<CoffeeMaker>(wallet);
        }

        public async Task<CoffeeMakerProgram> GetCoffeeMakerProgramDetails(string wallet, int program)
        {
            return await _contract.GetFunction("getCoffeeMakerProgramDetails").CallDeserializingToObjectAsync<CoffeeMakerProgram>(wallet, program);
        }

        public async Task<int> GetCoffeeMakerProgramCount(string wallet)
        {
            return await _contract.GetFunction("getCoffeeMakerProgramCount").CallAsync<int>(wallet);
        }
    }
}