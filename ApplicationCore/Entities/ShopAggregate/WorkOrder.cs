using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace RepairMarketPlace.ApplicationCore.Entities.ShopAggregate
{
    public class WorkOrder : BaseEntity
    {
        [Required]
        public Guid ConstumerId { get; private set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RequestDate { get; private set; }
        [DataType(DataType.DateTime)]
        public DateTime CompletionDate { get; private set; }
        public WorkOrderStatus WorkOrderStatus { get; private set; }
        public string WorkRemarks { get; private set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; private set; }

        internal WorkOrder(Guid constumerId, DateTime requestDate,
                 WorkOrderStatus workOrderStatus, string workRemarks, decimal price,
                 int shopId, Shop shop)
        {
            ConstumerId = constumerId;
            RequestDate = requestDate;
            WorkOrderStatus = workOrderStatus;
            WorkRemarks = workRemarks;
            Price = price;
            ShopId = shopId;
            Shop = shop;
        }

        //-----------------------------------------------
        // Relationships
        public int ShopId { get; private set; }
        public Shop Shop { get; set; }       
        /**
        private readonly List<RepairItem> _repairItems;
        IReadOnlyCollection<RepairItem> RepairItems => _repairItems?.AsReadOnly();
        **/
    }

    public enum WorkOrderStatus
    {
        WorkRequested,
        BudgetProposed,
        CostumerRejectedBudget,
        WorkInProgress,
        WorkCompleted,
        WorkIsLate,
        WorkCancelledByShop,
        WorkRefunded
    }
}
