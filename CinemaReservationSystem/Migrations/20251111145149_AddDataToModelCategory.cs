using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToModelCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into Categories (name, description, status) values ('Morbi odio odio, elementum eu, interdum eu, tincidunt in, leo.', 'Nulla suscipit ligula in lacus. Curabitur at ipsum ac tellus semper interdum.', 1);
insert into Categories (name, description, status) values ('Fusce consequat.', 'Nulla mollis molestie lorem. Quisque ut erat.', 1);
insert into Categories (name, description, status) values ('Proin leo odio, porttitor id, consequat in, consequat ut, nulla.', 'In est risus, auctor sed, tristique in, tempus sit amet, sem. Fusce consequat. Nulla nisl.', 1);
insert into Categories (name, description, status) values ('Nulla ac enim.', 'Nunc purus. Phasellus in felis.', 1);
insert into Categories (name, description, status) values ('Suspendisse potenti.', 'Nullam varius. Nulla facilisi. Cras non velit nec nisi vulputate nonummy.', 1);
insert into Categories (name, description, status) values ('Suspendisse potenti.', 'Donec posuere metus vitae ipsum. Aliquam non mauris.', 1);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Categories WHERE name IN ('Morbi odio odio, elementum eu, interdum eu, tincidunt in, leo.', 'Fusce consequat.', 'Proin leo odio, porttitor id, consequat in, consequat ut, nulla.', 'Nulla ac enim.', 'Suspendisse potenti.');");
        }
    }
}
