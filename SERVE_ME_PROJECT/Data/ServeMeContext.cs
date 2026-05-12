using System;
using System.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using SERVE_ME_PROJECT.Models;



namespace SERVE_ME_PROJECT.Data
{
    // IdentityUser
    public class ServeMeContext : IdentityDbContext<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ServeMeContext(DbContextOptions<ServeMeContext> options) : base(options)
        {

        }
        ///////////////////DbSET//////////////////////
        public DbSet<ApplicationUser> TbUser { get; set; }
        public DbSet<CategoryModel> TbCategory { get; set; }
        public DbSet<CityModel> TbCity { get; set; }
        public DbSet<CommentModel> TbComment { get; set; }
        public DbSet<OrderModel> TbOrder { get; set; }
        public DbSet<OrderStatModel> TbOrderStatModel { get; set; }
        public DbSet<ServiceImgeModel> TbServiceImge { get; set; }
        public DbSet<ServiceModel> TbService { get; set; }
        public DbSet<ServiceProviderModel> TbServiceProvider { get; set; }
        public DbSet<TypeserviceModel> TbTypeservice { get; set; }
        public DbSet<BannerModel> TbBanner { get; set; }
        public DbSet<GeneralSettingsModel> TbGeneralSettings { get; set; }
        public DbSet<ContactUsModel> TbContactUs { get; set; }
        public DbSet<BlogModel> TbBlog { get; set; }
        // جداول المحفظة
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<DepositRequest> DepositRequests { get; set; }
        public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }
        public DbSet<WalletTransfer> WalletTransfers { get; set; }
        public DbSet<RefundRequest> RefundRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // علاقات التعليقات
            modelBuilder.Entity<CommentModel>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقات الطلبات
            modelBuilder.Entity<OrderModel>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderModel>()
                .HasOne(o => o.Service)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقات الخدمات
            modelBuilder.Entity<ServiceModel>()
                .HasOne(s => s.Category)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ServiceModel>()
                .HasOne(s => s.City)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.CityId)
                .OnDelete(DeleteBehavior.Restrict);

            // علاقات المحفظة: التحويلات
            modelBuilder.Entity<WalletTransfer>()
                .HasOne(w => w.SenderUser)
                .WithMany()
                .HasForeignKey(w => w.SenderUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WalletTransfer>()
                .HasOne(w => w.ReceiverUser)
                .WithMany()
                .HasForeignKey(w => w.ReceiverUserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    } 
}
