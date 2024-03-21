using System;
using System.Collections.Generic;

namespace Paymaster.Payments.Data.Payments;

public partial class Payment
{
    public int Id { get; set; }

    /// <summary>
    /// Тип платежа. Ссылка на справочник
    /// </summary>
    public int PaymentTypesId { get; set; }

    public int? PaymentFilesId { get; set; }

    /// <summary>
    /// уникальный идентификатор платежа в банке
    /// </summary>
    public long? PaymentId { get; set; }

    /// <summary>
    /// номер платежа
    /// </summary>
    public int? PaymentNumber { get; set; }

    /// <summary>
    /// дата платежа
    /// </summary>
    public DateTime PaymentDate { get; set; }

    /// <summary>
    /// дата проведения платежа
    /// </summary>
    public DateTime? DateEnter { get; set; }

    /// <summary>
    /// Сумма платежа в копейках
    /// </summary>
    public long Summa { get; set; }

    /// <summary>
    /// Сумма НДС в копейках
    /// </summary>
    public long? SummaNds { get; set; }

    /// <summary>
    /// полная сумма транзакций, по которым формируется платеж
    /// </summary>
    public long? SummaFull { get; set; }

    /// <summary>
    /// сумма комиссии шлюза с транзакций, по которым формируется платеж
    /// </summary>
    public long? SummaComm { get; set; }

    /// <summary>
    /// НДС с суммы комиссии
    /// </summary>
    public long? SummaCommNds { get; set; }

    /// <summary>
    /// назначение платежа
    /// </summary>
    public string Purpose { get; set; } = null!;

    /// <summary>
    /// Номер счета получателя или ИНН плательщика (идентификатор в нашей БД)
    /// </summary>
    public string Account { get; set; } = null!;

    public int? GatewaysId { get; set; }

    public int? EnterpriseId { get; set; }

    /// <summary>
    /// Дата добавления записи
    /// </summary>
    public DateTime DateAdd { get; set; }

    public int UserId { get; set; }

    /// <summary>
    /// Для записей, которые формирует система: 1 если бухгалтер лично акцептовал платеж, 0 иначе. Если запись загружена, то 0 - не обработана, 1 обработана
    /// </summary>
    public bool CheckedByAccounter { get; set; }

    /// <summary>
    /// DATE_FROM и DATE_TO - диапазон дат и времени, за которые сформирован платеж оператору в местном времени оператора (с учетом часового пояса оператора)
    /// </summary>
    public DateTime? DateFrom { get; set; }

    /// <summary>
    /// DATE_FROM и DATE_TO - диапазон дат и времени, за которые сформирован платеж оператору в местном времени оператора (с учетом часового пояса оператора)
    /// </summary>
    public DateTime? DateTo { get; set; }

    public string? Notes { get; set; }

    public string? Inn { get; set; }

    public int? ContractsId { get; set; }

    /// <summary>
    /// Банк для проведения платежа
    /// </summary>
    public int? Bank { get; set; }

    public long? ExtDocId { get; set; }

    /// <summary>
    /// ID платежа головного субагента, который был создан при создании данного платежа для уменьшения баланса головного субагента (случай ENTERPRISE.ADD_BALANCE_TYPE = 2).
    /// </summary>
    public int? ParentPaymentId { get; set; }

    public string? ShopPosCode { get; set; }

    public DateTime? DatePayEnterprise { get; set; }

    public string? CashRegister { get; set; }

    public int? RegionalManagerId { get; set; }

    public int? EncashmentTypeId { get; set; }

    public int? Acts2Id { get; set; }
}
