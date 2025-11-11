using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaReservationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToModelMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"insert into Movies (name, description, price, status, releaseDate, mainImg, categoryId, cinemaId) values ('Mauris sit amet eros.', 'Donec dapibus. Duis at velit eu est congue elementum. In hac habitasse platea dictumst.', 600.32, 1, '2003-07-14 00:26:53', 'movie3.jpg', 2, 2);
insert into Movies (name, description, price, status, releaseDate, mainImg, categoryId, cinemaId) values ('Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi, in porttitor pede justo eu massa.', 'Nullam porttitor lacus at turpis. Donec posuere metus vitae ipsum.', 1027.32, 1, '2022-02-06 00:44:17', 'movie1.jpg', 2, 2);
insert into Movies (name, description, price, status, releaseDate, mainImg, categoryId, cinemaId) values ('Ut at dolor quis odio consequat varius.', 'In hac habitasse platea dictumst. Etiam faucibus cursus urna. Ut tellus.', 1362.63, 0, '2009-12-13 15:29:02', 'movie2.jpg', 6, 6);
insert into Movies (name, description, price, status, releaseDate, mainImg, categoryId, cinemaId) values ('Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue.', 'Nam nulla. Integer pede justo, lacinia eget, tincidunt eget, tempus vel, pede. Morbi porttitor lorem id ligula. Suspendisse ornare consequat lectus.', 1483.73, 0, '2002-05-10 21:39:33', 'movie2.jpg', 2, 4);
insert into Movies (name, description, price, status, releaseDate, mainImg, categoryId, cinemaId) values ('Quisque porta volutpat erat.', 'Duis aliquam convallis nunc.', 709.76, 1, '2002-04-03 22:32:49', 'movie1.jpg', 5, 1);
insert into Movies (name, description, price, status, releaseDate, mainImg, categoryId, cinemaId) values ('Quisque arcu libero, rutrum ac, lobortis vel, dapibus at, diam.', 'Suspendisse potenti. Cras in purus eu magna vulputate luctus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.', 979.56, 0, '2017-07-29 05:06:21', 'movie1.jpg', 5, 3);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Movies WHERE name IN ('Mauris sit amet eros.', 'Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi, in porttitor pede justo eu massa.', 'Ut at dolor quis odio consequat varius.', 'Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue.', 'Quisque porta volutpat erat.', 'Quisque arcu libero, rutrum ac, lobortis vel, dapibus at, diam.');");
        }
    }
}
