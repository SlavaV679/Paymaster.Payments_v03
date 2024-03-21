using System;
using System.Collections.Generic;

namespace Paymaster.Payments.Data.Payments;

public partial class Acts2
{
    public int ContractsId { get; set; }

    /// <summary>
    /// Undefined = -1, Calculated = 1, Accepted = 2, Incorrect = 3, NotMailed = 4, NotCalculated = 0,//Это состояние может быть только в sql. В выборку такие акты не попадают
    /// </summary>
    public int State { get; set; }

    public DateTime StateDate { get; set; }

    public DateTime DateStart { get; set; }

    public DateTime DateEnd { get; set; }

    public string DatePeriod { get; set; } = null!;

    public int TransactionCount { get; set; }

    public long TotalSum { get; set; }

    public long SubagentReward { get; set; }

    public long UpperComission { get; set; }

    public long AgentPartInUpComission { get; set; }

    public string? Notes { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdateUserId { get; set; }

    public int? IsManuallyEdit { get; set; }

    public DateTime? DateReceive { get; set; }

    public DateTime? DateTransactionChange { get; set; }

    public DateTime? ExportDate { get; set; }

    public int? EnterpriseId { get; set; }

    public long InsuredSumWoTax { get; set; }

    public long InsuredTax { get; set; }

    public int EmailPeriodicityOffset { get; set; }

    public DateTime EmailSendDate { get; set; }

    public long TotalProvisionSumm { get; set; }

    public int Id { get; set; }

    public int EquipmentRent { get; set; }

    public int? ActTypeId { get; set; }

    public int? GatewayId { get; set; }

    public string? MerchantSite { get; set; }

    public long? PaymentSum { get; set; }

    public int? PaymentCount { get; set; }

    public long? PaymentReward { get; set; }

    public bool? IsCorrect { get; set; }
}
