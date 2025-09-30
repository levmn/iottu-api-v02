using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Application.Services
{
    public class MotoService : IMotoService
    {
        private readonly IMotoRepository _motoRepository;

        public MotoService(IMotoRepository motoRepository)
        {
            _motoRepository = motoRepository;
        }

        public async Task<IEnumerable<MotoDto>> GetAllAsync(int page, int pageSize)
        {
            var motos = await _motoRepository.GetAllAsync(page, pageSize);
            return motos.Select(m => new MotoDto
            {
                Id = m.Id,
                Placa = m.Placa,
                Modelo = m.Modelo,
                Chassi = m.Chassi,
                NumeroMotor = m.NumeroMotor,
                StatusId = m.StatusId,
                TagId = m.TagId,
            });
        }

        public async Task<int> CountAsync()
        {
            return await _motoRepository.CountAsync();
        }

        public async Task<MotoDto?> GetByIdAsync(Guid id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null) return null;

            return new MotoDto
            {
                Id = moto.Id,
                Placa = moto.Placa,
                Modelo = moto.Modelo,
                Chassi = moto.Chassi,
                NumeroMotor = moto.NumeroMotor,
                StatusId = moto.StatusId,
                TagId = moto.TagId,
                PatioId = moto.PatioId
            };
        }

        public async Task<MotoDto> CreateAsync(CreateMotoDto dto)
        {
            var moto = new Moto(dto.Placa, dto.Modelo, dto.Chassi, dto.NumeroMotor, dto.StatusId, dto.TagId, dto.PatioId);
            await _motoRepository.AddAsync(moto);

            return new MotoDto
            {
                Id = moto.Id,
                Placa = moto.Placa,
                Modelo = moto.Modelo,
                Chassi = moto.Chassi,
                NumeroMotor = moto.NumeroMotor,
                StatusId = moto.StatusId,
                TagId = moto.TagId,
                PatioId = moto.PatioId
            };
        }

        public async Task<MotoDto?> UpdateAsync(Guid id, UpdateMotoDto dto)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null) return null;

            moto.Atualizar(dto.Modelo, dto.Chassi, dto.NumeroMotor, dto.StatusId);
            await _motoRepository.UpdateAsync(moto);

            return new MotoDto
            {
                Id = moto.Id,
                Placa = moto.Placa,
                Modelo = moto.Modelo,
                Chassi = moto.Chassi,
                NumeroMotor = moto.NumeroMotor,
                StatusId = moto.StatusId,
                TagId = moto.TagId,
                PatioId = moto.PatioId
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var moto = await _motoRepository.GetByIdAsync(id);
            if (moto == null) return false;

            await _motoRepository.DeleteAsync(id);
            return true;
        }
    }
}
