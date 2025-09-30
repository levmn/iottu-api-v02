using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Application.Services
{
    public class PatioService : IPatioService
    {
        private readonly IPatioRepository _patioRepository;

        public PatioService(IPatioRepository patioRepository)
        {
            _patioRepository = patioRepository;
        }

        public async Task<IEnumerable<PatioDto>> GetAllAsync(int page, int pageSize)
        {
            var patios = await _patioRepository.GetAllAsync(page, pageSize);
            return patios.Select(p => new PatioDto
            {
                Id = p.Id,
                Cep = p.Cep,
                Numero = p.Numero,
                Cidade = p.Cidade,
                Estado = p.Estado,
                Capacidade = p.Capacidade
            });
        }

        public async Task<int> CountAsync()
        {
            return await _patioRepository.CountAsync();
        }

        public async Task<PatioDto?> GetByIdAsync(Guid id)
        {
            var p = await _patioRepository.GetByIdAsync(id);
            if (p == null) return null;
            return new PatioDto
            {
                Id = p.Id,
                Cep = p.Cep,
                Numero = p.Numero,
                Cidade = p.Cidade,
                Estado = p.Estado,
                Capacidade = p.Capacidade
            };
        }

        public async Task<PatioDto> CreateAsync(CreatePatioDto dto)
        {
            var p = new Patio
            {
                Id = Guid.NewGuid(),
                Cep = dto.Cep,
                Numero = dto.Numero,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Capacidade = dto.Capacidade
            };
            await _patioRepository.AddAsync(p);
            return await GetByIdAsync(p.Id) ?? new PatioDto { Id = p.Id, Cep = p.Cep, Numero = p.Numero, Cidade = p.Cidade, Estado = p.Estado, Capacidade = p.Capacidade };
        }

        public async Task<PatioDto?> UpdateAsync(Guid id, UpdatePatioDto dto)
        {
            var p = await _patioRepository.GetByIdAsync(id);
            if (p == null) return null;

            p.Cep = dto.Cep;
            p.Numero = dto.Numero;
            p.Cidade = dto.Cidade;
            p.Estado = dto.Estado;
            p.Capacidade = dto.Capacidade;

            await _patioRepository.UpdateAsync(p);
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var p = await _patioRepository.GetByIdAsync(id);
            if (p == null) return false;
            await _patioRepository.DeleteAsync(id);
            return true;
        }
    }
}