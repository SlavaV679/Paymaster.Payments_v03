using Microsoft.EntityFrameworkCore;

namespace Paymaster.Payments.Data.Payments;

public partial class PaymentsContext : DbContext
{
    public PaymentsContext()
    {
    }

    public PaymentsContext(DbContextOptions<PaymentsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acts2> Acts2s { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Cyrillic_General_CI_AI");

        modelBuilder.Entity<Acts2>(entity =>
        {
            entity.ToTable("ACTS2");

            entity.HasIndex(e => e.ActTypeId, "IX_ActTypeID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActTypeId).HasColumnName("ActTypeID");
            entity.Property(e => e.ContractsId).HasColumnName("CONTRACTS_ID");
            entity.Property(e => e.DateEnd)
                .HasColumnType("datetime")
                .HasColumnName("dateEnd");
            entity.Property(e => e.DatePeriod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("datePeriod");
            entity.Property(e => e.DateReceive)
                .HasColumnType("datetime")
                .HasColumnName("DATE_RECEIVE");
            entity.Property(e => e.DateStart)
                .HasColumnType("datetime")
                .HasColumnName("dateStart");
            entity.Property(e => e.DateTransactionChange)
                .HasColumnType("datetime")
                .HasColumnName("DATE_TRANSACTION_CHANGE");
            entity.Property(e => e.EmailPeriodicityOffset).HasColumnName("emailPeriodicityOffset");
            entity.Property(e => e.EmailSendDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("emailSendDate");
            entity.Property(e => e.EnterpriseId).HasColumnName("enterprise_id");
            entity.Property(e => e.ExportDate)
                .HasColumnType("datetime")
                .HasColumnName("EXPORT_DATE");
            entity.Property(e => e.GatewayId).HasColumnName("GATEWAY_ID");
            entity.Property(e => e.IsManuallyEdit)
                .HasDefaultValue(0)
                .HasColumnName("isManuallyEdit");
            entity.Property(e => e.MerchantSite).HasMaxLength(50);
            entity.Property(e => e.Notes)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("notes");
            entity.Property(e => e.State)
                .HasComment("Undefined = -1, Calculated = 1, Accepted = 2, Incorrect = 3, NotMailed = 4, NotCalculated = 0,//Это состояние может быть только в sql. В выборку такие акты не попадают")
                .HasColumnName("state");
            entity.Property(e => e.StateDate)
                .HasColumnType("datetime")
                .HasColumnName("stateDate");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("updateDate");
            entity.Property(e => e.UpdateUserId).HasColumnName("updateUserId");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).IsClustered(false);

            entity.ToTable("PAYMENTS", tb => tb.HasTrigger("RECALC_SUMMA_PAYMENTS"));

            entity.HasIndex(e => e.PaymentDate, "IX_PAYMENTS").HasFillFactor(80);

            entity.HasIndex(e => new { e.EnterpriseId, e.ContractsId }, "IX_PAYMENTS_1").HasFillFactor(80);

            entity.HasIndex(e => new { e.GatewaysId, e.ContractsId }, "IX_PAYMENTS_2").HasFillFactor(80);

            entity.HasIndex(e => e.ContractsId, "IX_PAYMENTS_3").HasFillFactor(80);

            entity.HasIndex(e => new { e.PaymentTypesId, e.DateEnter, e.ContractsId }, "IX_Payments4").HasFillFactor(80);

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComment("Номер счета получателя или ИНН плательщика (идентификатор в нашей БД)")
                .HasColumnName("ACCOUNT");
            entity.Property(e => e.Acts2Id).HasColumnName("ACTS2_ID");
            entity.Property(e => e.Bank)
                .HasDefaultValue(0)
                .HasComment("Банк для проведения платежа")
                .HasColumnName("BANK");
            entity.Property(e => e.CashRegister)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CASH_REGISTER");
            entity.Property(e => e.CheckedByAccounter)
                .HasComment("Для записей, которые формирует система: 1 если бухгалтер лично акцептовал платеж, 0 иначе. Если запись загружена, то 0 - не обработана, 1 обработана")
                .HasColumnName("CHECKED_BY_ACCOUNTER");
            entity.Property(e => e.ContractsId).HasColumnName("CONTRACTS_ID");
            entity.Property(e => e.DateAdd)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Дата добавления записи")
                .HasColumnType("datetime")
                .HasColumnName("DATE_ADD");
            entity.Property(e => e.DateEnter)
                .HasComment("дата проведения платежа")
                .HasColumnType("datetime")
                .HasColumnName("DATE_ENTER");
            entity.Property(e => e.DateFrom)
                .HasComment("DATE_FROM и DATE_TO - диапазон дат и времени, за которые сформирован платеж оператору в местном времени оператора (с учетом часового пояса оператора)")
                .HasColumnType("datetime")
                .HasColumnName("DATE_FROM");
            entity.Property(e => e.DatePayEnterprise)
                .HasColumnType("datetime")
                .HasColumnName("DATE_PAY_ENTERPRISE");
            entity.Property(e => e.DateTo)
                .HasComment("DATE_FROM и DATE_TO - диапазон дат и времени, за которые сформирован платеж оператору в местном времени оператора (с учетом часового пояса оператора)")
                .HasColumnType("datetime")
                .HasColumnName("DATE_TO");
            entity.Property(e => e.EncashmentTypeId).HasColumnName("ENCASHMENT_TYPE_ID");
            entity.Property(e => e.EnterpriseId).HasColumnName("ENTERPRISE_ID");
            entity.Property(e => e.ExtDocId).HasColumnName("EXT_DOC_ID");
            entity.Property(e => e.GatewaysId).HasColumnName("GATEWAYS_ID");
            entity.Property(e => e.Inn)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("INN");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("notes");
            entity.Property(e => e.ParentPaymentId)
                .HasComment("ID платежа головного субагента, который был создан при создании данного платежа для уменьшения баланса головного субагента (случай ENTERPRISE.ADD_BALANCE_TYPE = 2).")
                .HasColumnName("parent_payment_id");
            entity.Property(e => e.PaymentDate)
                .HasComment("дата платежа")
                .HasColumnType("datetime")
                .HasColumnName("PAYMENT_DATE");
            entity.Property(e => e.PaymentFilesId).HasColumnName("PAYMENT_FILES_ID");
            entity.Property(e => e.PaymentId)
                .HasComment("уникальный идентификатор платежа в банке")
                .HasColumnName("PAYMENT_ID");
            entity.Property(e => e.PaymentNumber)
                .HasComment("номер платежа")
                .HasColumnName("PAYMENT_NUMBER");
            entity.Property(e => e.PaymentTypesId)
                .HasComment("Тип платежа. Ссылка на справочник")
                .HasColumnName("PAYMENT_TYPES_ID");
            entity.Property(e => e.Purpose)
                .HasMaxLength(210)
                .IsUnicode(false)
                .HasComment("назначение платежа")
                .HasColumnName("PURPOSE");
            entity.Property(e => e.RegionalManagerId).HasColumnName("REGIONAL_MANAGER_ID");
            entity.Property(e => e.ShopPosCode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SHOP_POS_CODE");
            entity.Property(e => e.Summa)
                .HasComment("Сумма платежа в копейках")
                .HasColumnName("SUMMA");
            entity.Property(e => e.SummaComm)
                .HasComment("сумма комиссии шлюза с транзакций, по которым формируется платеж")
                .HasColumnName("SUMMA_COMM");
            entity.Property(e => e.SummaCommNds)
                .HasComment("НДС с суммы комиссии")
                .HasColumnName("SUMMA_COMM_NDS");
            entity.Property(e => e.SummaFull)
                .HasComment("полная сумма транзакций, по которым формируется платеж")
                .HasColumnName("SUMMA_FULL");
            entity.Property(e => e.SummaNds)
                .HasComment("Сумма НДС в копейках")
                .HasColumnName("SUMMA_NDS");
            entity.Property(e => e.UserId)
                .HasDefaultValue(-1)
                .HasColumnName("USER_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
