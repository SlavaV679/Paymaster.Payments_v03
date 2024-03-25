using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Paymaster.Payments.Data.Payments;
using Paymaster.Payments.Domain.Config;
using Paymaster.Payments.Domain.Models;
using Paymaster.Payments.Logic.Interfaces;
using System.Data;

namespace Paymaster.Payments.Logic.Repository
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly Configuration _config;

        public PaymentsRepository(Configuration configuration)
        {
            _config = configuration;
        }

        private PaymentsContext GetDb()
        {
            var optionsBuilder = new DbContextOptionsBuilder<PaymentsContext>();
            var options = optionsBuilder
                .UseSqlServer(_config.PaymentsConnectionString)
                .Options;

            return new PaymentsContext(options);
        }

        public Acts2? GetAct()
        {
            using (var db = GetDb())
            {
                return db.Acts2s.FirstOrDefault();
            }
        }

        public decimal GetCurrencyRateByDate(DateTime dateCurrencyRate)
        {
            var currFrom = new SqlParameter("@CurrFrom", "MDL");
            var currTo = new SqlParameter("@CurrTo", "USM");
            var date = new SqlParameter("@Date", dateCurrencyRate);
            var rate = new SqlParameter("@Rate", SqlDbType.Decimal) { Direction = ParameterDirection.Output, Precision = 10, Scale = 8 };
            var useRateNumber = new SqlParameter("@UseRateNumber", 1);

            using (var context = GetDb())
            {
                try
                {
                    var v = context.Database.ExecuteSqlRaw("dbo.[GetCurrencyRateByDate] @CurrFrom, @CurrTo, @Date, @Rate OUT, @UseRateNumber",
                        currFrom, currTo, date, rate, useRateNumber);
                }
                catch (Exception ex)
                {

                    throw;
                }

                if (rate.Value == DBNull.Value)
                    throw new Exception($"GetCurrencyRateByDate, не найдена запись в таблице 'CURRENCY_RATE' или Rate = null. Передаваемое значение @Date = '{dateCurrencyRate}'.");

                return (decimal)rate.Value;
            }
        }

        public int MakeActPaymentFromPMToBalance(ActPayment actPayment)
        {
            var paymentTypeId = new SqlParameter("@PaymentTypeId", actPayment.PaymentTypeId);
            var summa = new SqlParameter("@Summa", actPayment.Summa);
            var сhecked = new SqlParameter("@Checked", actPayment.CheckedByAccounter);
            var purpose = new SqlParameter("@Purpose", actPayment.Purpose);
            var paymentDate = new SqlParameter("@PaymentDate", actPayment.PaymentDate);
            var inn = new SqlParameter("@Inn", actPayment.Inn);
            var currency = new SqlParameter("@Currency", actPayment.Currency);
            var extDocId = new SqlParameter("@ExtDocId", actPayment.ExtDocId);
            var limitCover = new SqlParameter("@LimitCover", actPayment.LimitCover);

            SqlParameter paramId = new SqlParameter("@Id", SqlDbType.Int) { Direction = ParameterDirection.Output };
            SqlParameter paramDateChange = new SqlParameter("@DateChange", SqlDbType.DateTime) { Direction = ParameterDirection.Output, Value = DateTime.Now };
            SqlParameter paramIsOk = new SqlParameter("@IsOk", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            SqlParameter paramErrorCode = new SqlParameter("@ErrorCode", SqlDbType.Int) { Direction = ParameterDirection.Output };
            SqlParameter parentId = new SqlParameter("@ID_PARENT", SqlDbType.Int) { Direction = ParameterDirection.Output};

            using (var context = GetDb())
            {
                try
                {
                    #region CreateCommand from EF
                    //var connection = context.Database.GetDbConnection();
                    //using(var command  = connection.CreateCommand())
                    //{
                    //    command.CommandText = "uiPAYMENTS_change"; // Название вашей хранимой процедуры
                    //    command.CommandType = CommandType.StoredProcedure;
                    //    command.Parameters.Add(paramId);
                    //    command.Parameters.Add(paramDateChange);
                    //    command.Parameters.Add(paramIsOk);
                    //    command.Parameters.Add(parentId);
                    //    command.Parameters.Add(paramErrorCode);
                    //    command.Parameters.Add(paymentTypeId);
                    //    command.Parameters.Add(summa);
                    //    command.Parameters.Add(summaFull);
                    //    command.Parameters.Add(summaComm);
                    //    command.Parameters.Add(cONTRACTS_ID);
                    //    command.Parameters.Add(parametrChecked);
                    //    command.Parameters.Add(purpose);
                    //    command.Parameters.Add(notes);
                    //    command.Parameters.Add(inn);
                    //    command.Parameters.Add(bank);
                    //    command.Parameters.Add(cashRegister);
                    //    command.Parameters.Add(extDocId);
                    //    try
                    //    {
                    //        connection.Open();
                    //        var trancation = connection.BeginTransaction();
                    //        command.Transaction = trancation;
                    //        try
                    //        {
                    //            command.ExecuteNonQuery();
                    //            isOk = (bool)paramIsOk.Value;
                    //            if (isOk) { }
                    //            //{
                    //            //    if (item.Type.Id == 17)
                    //            //    {
                    //            //        if (item.ChildEnterprise != null)
                    //            //        {
                    //            //            command.Parameters["@Summa"].Value = item.Summa;
                    //            //            command.Parameters["@CONTRACTS_ID"].Value = item.ChildContract.Id;
                    //            //            command.Parameters["@ID_PARENT"].Value = (int)parentId.Value;
                    //            //            command.ExecuteNonQuery();
                    //            //            isOk = (bool)paramIsOk.Value;
                    //            //            if (!isOk)
                    //            //            {
                    //            //                int errorCode = (int)parameterErrorCode.Value;
                    //            //                throw new ApplicationException(GetMessageForErrorCode(errorCode));
                    //            //            }
                    //            //        }
                    //            //    }
                    //            //    item.Id = (int)paramId.Value;
                    //            //    item.DateChange = (DateTime)paramDateChange.Value;
                    //            //    trancation.Commit();
                    //            //}
                    //            else
                    //            {
                    //                int errorCode = (int)paramErrorCode.Value;
                    //                //throw new ApplicationException(GetMessageForErrorCode(errorCode));
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            trancation.Rollback();
                    //            throw new ApplicationException(string.Format("При сохранении нового платежа произошла ошибка. {0}", ex.Message), ex);
                    //        }
                    //    }
                    //    catch (SqlException exception)
                    //    {
                    //        string message = "";// SqlErrorCodesTranslator.TranslateCode(exception.Number);
                    //        if (string.IsNullOrEmpty(message))
                    //        {
                    //            message = exception.Message;
                    //        }
                    //        throw new ApplicationException(string.Format("При сохранении нового платежа произошла ошибка. {0}", message), exception);
                    //    }

                    //    var result = command.ExecuteReader();

                    //    // Здесь вы можете обработать результаты выполнения
                    //    // Например, прочитать данные, если хранимая процедура что-то возвращает

                    //    result.Close(); // Не забываем закрыть reader
                    //}

                    #endregion

                    var v = context.Database.ExecuteSqlRaw("dbo.[uiPAYMENTS_change] " +
                        "@PaymentTypeId=@PaymentTypeId,     @Summa=@Summa,                  @Checked=@Checked," +
                        "@Purpose=@Purpose,                 @PaymentDate=@PaymentDate,      @Inn=@Inn," +
                        "@Currency=@Currency,               @ExtDocId=@ExtDocId,            @LimitCover=@LimitCover," +
                        "@Id=@Id OUT,       @DateChange=@DateChange OUT,        @IsOk=@IsOk OUT,      @ErrorCode=@ErrorCode OUT,     @ID_PARENT=@ID_PARENT OUT",

                        paymentTypeId, summa, сhecked, 
                        purpose, paymentDate, inn,
                        currency, extDocId, limitCover,                        
                        paramId, paramDateChange, paramIsOk, paramErrorCode, parentId);
                }
                catch (Exception ex)
                {

                    throw;
                }

                if (paramIsOk.Value == DBNull.Value)
                    throw new Exception($"Store procedure uiPAYMENTS_change failed.");
                if ((bool)paramIsOk.Value == false)
                {
                    
                }
                return (int)paramErrorCode.Value;
            }
        }
    }
}
