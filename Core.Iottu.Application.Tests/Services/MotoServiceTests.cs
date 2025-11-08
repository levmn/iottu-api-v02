using Core.Iottu.Application.Services;
using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared.Iottu.Contracts.DTOs;
using Xunit;

namespace Core.Iottu.Application.Tests.Services
{
    public class MotoServiceTests
    {
        private readonly Mock<IMotoRepository> _motoRepositoryMock;
        private readonly MotoService _service;

        public MotoServiceTests()
        {
            _motoRepositoryMock = new Mock<IMotoRepository>();
            _service = new MotoService(_motoRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            var motos = new List<Moto>
            {
                new Moto("ABC1234", "CG 160", "CHS001", "MTR001", 1, Guid.NewGuid(), Guid.NewGuid()),
                new Moto("XYZ5678", "YBR 150", "CHS002", "MTR002", 2, Guid.NewGuid(), Guid.NewGuid())
            };
            _motoRepositoryMock.Setup(r => r.GetAllAsync(1, 10)).ReturnsAsync(motos);

            var result = (await _service.GetAllAsync(1, 10)).ToList();

            result.Should().HaveCount(2);
            result[0].Modelo.Should().Be("CG 160");
            result[1].Placa.Should().Be("XYZ5678");
        }

        [Fact]
        public async Task CountAsync_ShouldReturnRepositoryCount()
        {
            _motoRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(5);

            var result = await _service.CountAsync();

            result.Should().Be(5);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenFound()
        {
            var id = Guid.NewGuid();
            var moto = new Moto("ABC1234", "CG 160", "CHS001", "MTR001", 1, Guid.NewGuid(), Guid.NewGuid()) { Id = id };
            _motoRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(moto);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result!.Modelo.Should().Be("CG 160");
            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _motoRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Moto?)null);

            var result = await _service.GetByIdAsync(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddNewMotoAndReturnDto()
        {
            var dto = new CreateMotoDto
            {
                Placa = "DEF5678",
                Modelo = "Fan 160",
                Chassi = "CHS999",
                NumeroMotor = "MTR999",
                StatusId = 1,
                TagId = Guid.NewGuid(),
                PatioId = Guid.NewGuid()
            };

            Moto? storedMoto = null;
            _motoRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Moto>()))
                .Callback<Moto>(m => storedMoto = m)
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.Placa.Should().Be("DEF5678");
            storedMoto.Should().NotBeNull();
            _motoRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Moto>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyMotoAndReturnUpdatedDto()
        {
            var id = Guid.NewGuid();
            var moto = new Moto("ABC1234", "CG 160", "CHS001", "MTR001", 1, Guid.NewGuid(), Guid.NewGuid()) { Id = id };
            _motoRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(moto);
            _motoRepositoryMock.Setup(r => r.UpdateAsync(moto)).Returns(Task.CompletedTask);

            var dto = new UpdateMotoDto
            {
                Modelo = "Titan 160",
                Chassi = "CHS222",
                NumeroMotor = "MTR222",
                StatusId = 2
            };

            var result = await _service.UpdateAsync(id, dto);

            result.Should().NotBeNull();
            result!.Modelo.Should().Be("Titan 160");
            _motoRepositoryMock.Verify(r => r.UpdateAsync(moto), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenMotoNotFound()
        {
            var id = Guid.NewGuid();
            _motoRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Moto?)null);

            var result = await _service.UpdateAsync(id, new UpdateMotoDto());

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAndReturnTrue_WhenMotoExists()
        {
            var id = Guid.NewGuid();
            var moto = new Moto("ABC1234", "CG 160", "CHS001", "MTR001", 1, Guid.NewGuid(), Guid.NewGuid()) { Id = id };
            _motoRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(moto);
            _motoRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(id);

            result.Should().BeTrue();
            _motoRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenMotoNotFound()
        {
            var id = Guid.NewGuid();
            _motoRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Moto?)null);

            var result = await _service.DeleteAsync(id);

            result.Should().BeFalse();
        }
    }
}
