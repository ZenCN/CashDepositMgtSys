namespace Service.Model.Interface
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class INTERFACE_MIO_BATCH_BZJ
    {
        [Key]
        public int BatchId { get; set; }

        [Required]
        [StringLength(1)]
        public string MioType { get; set; }

        [Required]
        [StringLength(10)]
        public string FromSys { get; set; }

        [Required]
        [StringLength(30)]
        public string FromBatchNo { get; set; }

        public int DataCnt { get; set; }

        public decimal SumAmnt { get; set; }

        public int BatchStatus { get; set; }

        public DateTime GenerateTime { get; set; }

        [Required]
        [StringLength(20)]
        public string GenerateBy { get; set; }

        [StringLength(2)]
        public string MioChannel { get; set; }

        [StringLength(50)]
        public string PlatBatchNo { get; set; }

        public DateTime? AppvTime { get; set; }

        [StringLength(20)]
        public string AppvBy { get; set; }

        public DateTime? SendTime { get; set; }

        [StringLength(50)]
        public string SendFile { get; set; }

        [StringLength(20)]
        public string SendBy { get; set; }

        [StringLength(50)]
        public string RcvFile { get; set; }

        public DateTime? RcvTime { get; set; }

        [StringLength(100)]
        public string Remark { get; set; }

        [StringLength(20)]
        public string Reserved { get; set; }
    }
}
