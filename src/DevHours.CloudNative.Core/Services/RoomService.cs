using DevHours.CloudNative.Domain;
using DevHours.CloudNative.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DevHours.CloudNative.Core.Services
{
    public class RoomService
    {
        private readonly ILogger logger;
        private readonly IDataRepository<Room> repository;

        public RoomService(ILogger<RoomService> logger, IDataRepository<Room> repository)
            => (this.logger, this.repository) = (logger, repository);

        public IQueryable<Room> Query() => repository.Query();

        public ValueTask<Room> GetRoomAsync(int roomId, CancellationToken token = default) => repository.GetAsync(roomId, token);

        public Task<Room> AddRoomAsync(Room room, CancellationToken token = default)
            => repository.AddAsync(room, token);

        public async Task UpdateRoomAsync(Room room, CancellationToken token = default)
        {
            var stored = await repository.GetAsync(room.Id);

            if (stored is null)
            {
                throw new NullReferenceException();
            }

            await repository.UpdateAsync(room, token);
        }

        public async Task DeleteRoomAsync(int roomId, CancellationToken token = default)
        {
            var stored = await repository.GetAsync(roomId);

            if (stored is null)
            {
                throw new NullReferenceException();
            }

            await repository.DeleteAsync(stored, token);
        }
    }
}