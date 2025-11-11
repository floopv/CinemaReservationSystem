using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToModelCinema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into Cinemas (name, description, status, img) values ('Morbi ut odio.', 'Duis aliquam convallis nunc. Proin at turpis a pede posuere nonummy. Integer non velit.', 0, 'cinema3.jpg');
insert into Cinemas (name, description, status, img) values ('Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh.', 'Morbi ut odio.', 0, 'cinema3.jpg');
insert into Cinemas (name, description, status, img) values ('Pellentesque viverra pede ac diam.', 'Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis. Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci. Mauris lacinia sapien quis libero.', 0, 'cinema3.jpg');
insert into Cinemas (name, description, status, img) values ('Morbi non quam nec dui luctus rutrum.', 'In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo. Aliquam quis turpis eget elit sodales scelerisque.', 1, 'cinema1.jpg');
insert into Cinemas (name, description, status, img) values ('Aenean fermentum.', 'Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh. Quisque id justo sit amet sapien dignissim vestibulum.', 1, 'cinema2.jpg');
insert into Cinemas (name, description, status, img) values ('Pellentesque ultrices mattis odio.', 'Cras non velit nec nisi vulputate nonummy. Maecenas tincidunt lacus at velit. Vivamus vel nulla eget eros elementum pellentesque.', 0, 'cinema3.jpg');
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Cinemas WHERE name IN ('Morbi ut odio.', 'Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh.', 'Pellentesque viverra pede ac diam.', 'Morbi non quam nec dui luctus rutrum.', 'Aenean fermentum.', 'Pellentesque ultrices mattis odio.');");
        }
    }
}
