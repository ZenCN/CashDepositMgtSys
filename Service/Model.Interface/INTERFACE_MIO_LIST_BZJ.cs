namespace Service.Model.Interface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class INTERFACE_MIO_LIST_BZJ
    {
        [Key]
        public long LineId { get; set; }

        [Required]
        [StringLength(10)]
        public string FromSys { get; set; }

        [Required]
        [StringLength(50)]
        public string FromBatchNo { get; set; }

        [Required]
        [StringLength(30)]
        public string FromUniqLine { get; set; }

        [Required]
        [StringLength(6)]
        public string ClicBranch { get; set; }

        [Required]
        [StringLength(4)]
        public string BankCode { get; set; }

        [Required]
        [StringLength(30)]
        public string BankAcc { get; set; }

        [Required]
        [StringLength(40)]
        public string BankAccName { get; set; }

        [Column(TypeName = "money")]
        public decimal MioAmount { get; set; }

        public long? BatchId { get; set; }

        public DateTime ApplTime { get; set; }

        public DateTime? TreatTime { get; set; }

        public DateTime? FinishTime { get; set; }

        [Required]
        [StringLength(1)]
        public string ProcStatus { get; set; }

        public int MioStatus { get; set; }

        [StringLength(200)]
        public string MioStatusRemark { get; set; }

        [StringLength(20)]
        public string AccOpenProvince { get; set; }

        [StringLength(20)]
        public string AccOpenCity { get; set; }

        [StringLength(60)]
        public string AccOpenBank { get; set; }

        [Required]
        [StringLength(1)]
        public string AccBookOrCard { get; set; }

        [Required]
        [StringLength(1)]
        public string AccPersonOrCompany { get; set; }

        [Required]
        [StringLength(3)]
        public string AccCurrencyType { get; set; }

        [StringLength(256)]
        public string ErrMsg { get; set; }
    }
}
