using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToModelActor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into Actors (name, bio, img) values ('Vestibulum rutrum rutrum neque.', 'Suspendisse potenti. Nullam porttitor lacus at turpis.', '3.jpg');
insert into Actors (name, bio, img) values ('Morbi ut odio.', 'Maecenas pulvinar lobortis est. Phasellus sit amet erat. Nulla tempus. Vivamus in felis eu sapien cursus vestibulum.', '1.jpg');
insert into Actors (name, bio, img) values ('Nulla ut erat id mauris vulputate elementum.', 'Etiam pretium iaculis justo.', '1.jpg');
insert into Actors (name, bio, img) values ('Nam dui.', 'In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo. Aliquam quis turpis eget elit sodales scelerisque.', '3.jpg');
insert into Actors (name, bio, img) values ('Nulla nisl.', 'Proin risus. Praesent lectus.', '2.jpg');
insert into Actors (name, bio, img) values ('Donec ut mauris eget massa tempor convallis.', 'Sed sagittis. Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci.', '2.jpg');
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Actors WHERE name IN ('Vestibulum rutrum rutrum neque.', 'Morbi ut odio.', 'Nulla ut erat id mauris vulputate elementum.', 'Nam dui.', 'Nulla nisl.', 'Donec ut mauris eget massa tempor convallis.');");
        }
    }
}
