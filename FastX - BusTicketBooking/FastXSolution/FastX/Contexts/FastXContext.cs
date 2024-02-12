using FastX.Models;
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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // Configure relationships using Fluent API

        //    // User - Booking (One-to-Many)
        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Bookings)
        //        .WithOne(b => b.User)
        //        .HasForeignKey(b => b.UserId);

        //    // BusOperator - Bus (One-to-Many)
        //    modelBuilder.Entity<BusOperator>()
        //        .HasMany(op => op.Buses)
        //        .WithOne(b => b.BusOperator)
        //        .HasForeignKey(b => b.BusOperatorId);

        //    // Bus - Booking (One-to-Many)
        //    modelBuilder.Entity<Bus>()
        //        .HasMany(b => b.Bookings)
        //        .WithOne(b => b.Bus)
        //        .HasForeignKey(b => b.BusId);

        //    //// Bus - BusAmenity (One-to-Many)
        //    //modelBuilder.Entity<Bus>()
        //    //    .HasMany(b => b.BusAmenities)
        //    //    .WithOne(ba => ba.Bus)
        //    //    .HasForeignKey(ba => ba.BusId);

        //    // Booking - Payment (One-to-One)
        //    modelBuilder.Entity<Booking>()
        //        .HasOne(b => b.Payment)
        //        .WithOne(p => p.Booking)
        //        .HasForeignKey<Payment>(p => p.BookingId);

        //    // Route - RouteStop (One-to-Many)
        //    modelBuilder.Entity<Routee>()
        //        .HasMany(r => r.RouteStops)
        //        .WithOne(rs => rs.Routee)
        //        .HasForeignKey(rs => rs.RouteId);

        //    // Stop - RouteStop (One-to-Many)
        //    modelBuilder.Entity<Stop>()
        //        .HasMany(s => s.RouteStops)
        //        .WithOne(rs => rs.Stop)
        //        .HasForeignKey(rs => rs.StopId);

        //    // Bus - Seat (One-to-Many)
        //    modelBuilder.Entity<Bus>()
        //        .HasMany(b => b.Seats)
        //        .WithOne(s => s.Bus)
        //        .HasForeignKey(s => s.BusId);
        //}

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
        }


    }
}