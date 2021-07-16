using DevHours.CloudNative.Core.Services;
using DevHours.CloudNative.Domain;
using DevHours.CloudNative.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevHours.CloudNative.Core.Test
{
    public class RoomBookingServiceTests
    {

        private readonly Mock<IDataRepository<Booking>> bookingRepository;
        private readonly Mock<IDataRepository<Room>> roomRepository;
        private readonly Mock<ILogger<RoomBookingService>> logger;

        private RoomBookingService roomBookingService;

        public RoomBookingServiceTests()
        {
            bookingRepository = new Mock<IDataRepository<Booking>>();
            roomRepository = new Mock<IDataRepository<Room>>();
            logger = new Mock<ILogger<RoomBookingService>>();
        }

        [Fact]
        public async Task Book_NoRoomExists_ShouldThrowException()
        {
            //Arrange
            var notExistingBookingRoom = new Booking();
            roomRepository
                .Setup(b => b.GetAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(null);
            roomBookingService = new RoomBookingService(logger.Object, bookingRepository.Object, roomRepository.Object);

            //Act
            await Assert.ThrowsAsync<NullReferenceException>(async () => await roomBookingService.Book(notExistingBookingRoom));
        }
    }
}
