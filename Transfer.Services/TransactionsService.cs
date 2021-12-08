using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Transfer.Core.Model.Base;
using Transfer.Data.Base;
using Transfer.Domain.Models;
using Transfer.RemoteServices.Base;
using Transfer.Services.Contracts;

namespace Transfer.Services
{
    public class TransactionsService : ITransactionsService
    {
        private readonly AppSettings _appSettings;
        private IRepositoryWrapper _repository;

        public TransactionsService(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _repository = repository;           
            _appSettings = appSettings.Value;
        }
        public Transactions CreateTransaction(Transactions transactions)
        {
            return _repository.Transactions.CreateTransaction(transactions);
        }

        public List<Transactions> GetTransactionsForTransfer()
        {
            return _repository.Transactions.GetTransactionsForTransfer();
        }

        public Token GenToken(string userName, string password)
        {
            return new Token(GenerateToken(userName + password));
        }        

        public List<Transactions> TransferInquiry(ConvertedInquiryParams convertedInquiryParams)
        {
            return _repository.Transactions.TransferInquiry(convertedInquiryParams);
        }

        private string GenerateToken(string claimValue, int? tokenValidateInMinutes = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.TokenSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, claimValue)
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenValidateInMinutes ?? _appSettings.TokenValidateInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool UpdateTransferValue(long id, string currency, string referenceId, string transactionStatus, string transferDescription, string transferStatus,int transferStatusId)
        {
            return _repository.Transactions.UpdateTransferValue( id,  currency,  referenceId,  transactionStatus,  transferDescription,  transferStatus, transferStatusId);
        }

        public List<Transactions> GetTranForUpdateTransferStatus()
        {
            return _repository.Transactions.GetTranForUpdateTransferStatus();
        }

        public bool Find(string iban)
        {
            return _repository.Transactions.Find(iban);
        }

        public bool UpdateTransferStatus(long id, string referenceId, int transferStatusId, string transferStatus, bool acceptable, bool cancelable, bool resumeable, bool suspendable)
        {
            return _repository.Transactions.UpdateTransferStatus(id, referenceId, transferStatusId, transferStatus, acceptable, cancelable, resumeable, suspendable);
        }
    }
}
