
using Wollo.Entities.Models;
using Wollo.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wollo.Data
{
     [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class PortalContext : DbContext
    {
        public PortalContext()
            : base("MySQLConnection")
        {
            Database.SetInitializer<PortalContext>(null);
        }

        #region Entity Sets

        public IDbSet<Transaction_History> TrsansactionHistorySet { get; set; }
        public IDbSet<Stock_Code> StockCodeSet { get; set; }
        public IDbSet<Queue_Status_Master> QueueStatusMasterSet { get; set; }
        public IDbSet<Wallet_Details> WalletDetailsSet { get; set; }
        public IDbSet<Wallet_Master> WalletMasterSet { get; set; }
        public IDbSet<Member_Stock_Details> MemberStockDetailsSet { get; set; }
        public IDbSet<Traded_History_Master> TradedHistoryMaster { get; set; }
        public IDbSet<Topup_Status_Master> TopupStatusMaster { get; set; }
        public IDbSet<Topup_History> TopupHistory { get; set; }
        public IDbSet<Cash_Transaction_History> CashTransactionHistory { get; set; }
        public IDbSet<Withdrawel_History_Details> WithdrawelHistoryDetails { get; set; }
        public IDbSet<Withdrawl_Fees> WithdrawlFees { get; set; }
        public IDbSet<Issue_Points_Master> IssuePointsMaster { get; set; }
        public IDbSet<Rule_Config_Settings> RuleConfigSettings { get; set; }
        public IDbSet<Administration_Fees> AdministrationFees { get; set; }
        public IDbSet<Holiday_Status_Master> HolidayStatusMaster { get; set; }
        public IDbSet<Holiday_Master> HolidayMaster { get; set; }
        public IDbSet<Trading_Time_Details> TradingTimeDetails { get; set; }
        public IDbSet<JobInfo> JobInfo { get; set; }
        public IDbSet<ScheduleType> ScheduleType { get; set; }
        public IDbSet<trading_days> TradingDays { get; set; }
        public IDbSet<AspnetUsers> Users { get; set; }
        public IDbSet<Units_Master> UnitMasters { get; set; }
        public IDbSet<User> PortalUsers { get; set; }
        public IDbSet<Country_Master> CountryMasters { get; set; }
        public IDbSet<Member_Status_Master> MemberStatusMasters { get; set; }
        public IDbSet<Audit_Log_Master> Logs { get; set; }
        public IDbSet<Issue_Cash_Transfer_Master> IssueCashTransferMastera { get; set; }

        public IDbSet<Market_Rate_Details> MarketRateDetails { get; set; }
        public IDbSet<Reward_Points_Transfer_Details> RewardPointsTransferDetails { get; set; }
        public IDbSet<Reward_Points_Transfer_Master> RewardPointsTransferMaster { get; set; }
        public IDbSet<Transfer_Action_Master> TransferActionMaster { get; set; }
        public IDbSet<Payment_Method> PaymentMethods { get; set; }
        public IDbSet<Cash_Transaction_Detail> TrsansactionDetailSet { get; set; }
        public IDbSet<Issue_Point_Transfer_Detail> PointTransferHistorySet { get; set; }
        public IDbSet<Wollo_Member_Details> WolloMemberDetails { get; set; }
        //public IDbSet<User> UserSet { get; set; }
        //public IDbSet<Role> RoleSet { get; set; }
        //public IDbSet<UserRole> UserRoleSet { get; set; }
        //public IDbSet<Customer> CustomerSet { get; set; }
        //public IDbSet<Movie> MovieSet { get; set; }
        //public IDbSet<Genre> GenreSet { get; set; }
        //public IDbSet<Stock> StockSet { get; set; }
        //public IDbSet<Rental> RentalSet { get; set; }
        public IDbSet<Error> ErrorSet { get; set; }
        #endregion

        public virtual void Commit()
        {
            base.SaveChanges();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //modelBuilder.Configurations.Add(new UserConfiguration());
            //modelBuilder.Configurations.Add(new UserRoleConfiguration());
            //modelBuilder.Configurations.Add(new RoleConfiguration());
            //modelBuilder.Configurations.Add(new CustomerConfiguration());
            //modelBuilder.Configurations.Add(new MovieConfiguration());
            //modelBuilder.Configurations.Add(new GenreConfiguration());
            //modelBuilder.Configurations.Add(new StockConfiguration());
            //modelBuilder.Configurations.Add(new RentalConfiguration());
        }
    }
}
