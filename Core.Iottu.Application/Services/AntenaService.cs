using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Application.Services
{
    public class AntenaService : IAntenaService
    {
        private readonly IAntenaRepository _antenaRepository;

        public AntenaService(IAntenaRepository antenaRepository)
        {
            _antenaRepository = antenaRepository;
        }

        public async Task<IEnumerable<AntenaDto>> GetAllAsync(int page, int pageSize)
        {
            var antenas = await _antenaRepository.GetAllAsync(page, pageSize);
            return antenas.Select(a => new AntenaDto
            {
                Id = a.Id,
                Localizacao = a.Localizacao,
                Identificador = a.Identificador
            });
        }

        public async Task<int> CountAsync()
        {
            return await _antenaRepository.CountAsync();
        }

        public async Task<AntenaDto?> GetByIdAsync(Guid id)
        {
            var antena = await _antenaRepository.GetByIdAsync(id);
            if (antena == null) return null;

            return new AntenaDto
            {
                Id = antena.Id,
                Localizacao = antena.Localizacao,
                Identificador = antena.Identificador
            };
        }

        public async Task<AntenaDto> CreateAsync(CreateAntenaDto dto)
        {
            var antena = new Antena(dto.Localizacao, dto.Identificador);
            await _antenaRepository.AddAsync(antena);

            return new AntenaDto
            {
                Id = antena.Id,
                Localizacao = antena.Localizacao,
                Identificador = antena.Identificador
            };
        }

        public async Task<AntenaDto?> UpdateAsync(Guid id, UpdateAntenaDto dto)
        {
            var antena = await _antenaRepository.GetByIdAsync(id);
            if (antena == null) return null;

            var updated = new Antena(dto.Localizacao, dto.Identificador);
            typeof(Antena).GetProperty("Id")!.SetValue(updated, antena.Id);
            await _antenaRepository.UpdateAsync(updated);

            return new AntenaDto
            {
                Id = updated.Id,
                Localizacao = updated.Localizacao,
                Identificador = updated.Identificador
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var antena = await _antenaRepository.GetByIdAsync(id);
            if (antena == null) return false;

            await _antenaRepository.DeleteAsync(id);
            return true;
        }
    }
}
