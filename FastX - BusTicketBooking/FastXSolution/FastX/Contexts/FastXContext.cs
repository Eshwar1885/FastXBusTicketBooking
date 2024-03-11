using FastX.Models;
using FastX.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastX.Contexts
{
    public class FastXContext : DbContext
    {
        public FastXContext(DbContextOptions<FastXContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data source=DESKTOP-7RD6P2A\\DEMOINSTANCE;User ID=sa;Password=Eshwar@123;Initial Catalog=dbfastxticket");
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<BusOperator> BusOperators { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<BusAmenity> BusAmenities { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Routee> Routees { get; set; }
        public DbSet<RouteStop> RouteStops { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<AllUser> AllUsers { get; set; }
        public DbSet<BusRoute> BusRoute { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Seat>()
            .HasKey(seat => new { seat.SeatId, seat.BusId });


            modelBuilder.Entity<Ticket>()
    .HasOne(ticket => ticket.Seat)
    .WithMany()
    .HasForeignKey(ticket => new { ticket.SeatId, ticket.BusId })
    .OnDelete(DeleteBehavior.NoAction); // Remove cascade delete



            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u!.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);





            ////Seeding data for testing purpose
            //modelBuilder.Entity<RegisterUserDTO>().HasData(
            //        new RegisterUserDTO { Username = "testuser", Password= "password",Role="user",Name="abc",ContactNumber="123"}
            //        );


            ////modelBuilder.Entity<Employee>().HasData(
            ////    new Employee
            ////    {
            ////        Id=101,
            ////        Name="Ramu",
            ////        DateOfBirth=new DateTime(),
            ////        Phone="9988776655",
            ////        Email="ramu@gmail.com",
            ////        Pic="./ramu.jpg",
            ////        DepartmentId=1
            ////    }
            ////    );
        }




    }
}