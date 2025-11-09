using Core.Iottu.Application.Services;
using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared.Iottu.Contracts.DTOs;
using Xunit;

namespace Core.Iottu.Application.Tests.Services
{
    public class PatioServiceTests
    {
        private readonly Mock<IPatioRepository> _patioRepositoryMock;
        private readonly PatioService _service;

        public PatioServiceTests()
        {
            _patioRepositoryMock = new Mock<IPatioRepository>();
            _service = new PatioService(_patioRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            var patios = new List<Patio>
            {
                new Patio { Id = Guid.NewGuid(), Cep = "12345-000", Numero = "100", Cidade = "São Paulo", Estado = "SP", Capacidade = 50 },
                new Patio { Id = Guid.NewGuid(), Cep = "22222-000", Numero = "200", Cidade = "Rio de Janeiro", Estado = "RJ", Capacidade = 70 }
            };

            _patioRepositoryMock.Setup(r => r.GetAllAsync(1, 10)).ReturnsAsync(patios);

            var result = (await _service.GetAllAsync(1, 10)).ToList();

            result.Should().HaveCount(2);
            result[0].Cidade.Should().Be("São Paulo");
            result[1].Estado.Should().Be("RJ");
        }

        [Fact]
        public async Task CountAsync_ShouldReturnRepositoryCount()
        {
            _patioRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(42);

            var count = await _service.CountAsync();

            count.Should().Be(42);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenFound()
        {
            var id = Guid.NewGuid();
            var patio = new Patio
            {
                Id = id,
                Cep = "88000-000",
                Numero = "15",
                Cidade = "Florianópolis",
                Estado = "SC",
                Capacidade = 100
            };
            _patioRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(patio);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result!.Cidade.Should().Be("Florianópolis");
            result.Capacidade.Should().Be(100);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _patioRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Patio?)null);

            var result = await _service.GetByIdAsync(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAndReturnNewPatio()
        {
            var dto = new CreatePatioDto
            {
                Cep = "12345-678",
                Numero = "99",
                Cidade = "Curitiba",
                Estado = "PR",
                Capacidade = 25
            };

            Patio? stored = null;

            _patioRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Patio>()))
                .Callback<Patio>(p => stored = p)
                .Returns(Task.CompletedTask);

            _patioRepositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(() => stored);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.Cidade.Should().Be("Curitiba");
            stored.Should().NotBeNull();
            stored!.Capacidade.Should().Be(25);
            _patioRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Patio>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExistingPatio()
        {
            var id = Guid.NewGuid();
            var patio = new Patio
            {
                Id = id,
                Cep = "00000-000",
                Numero = "1",
                Cidade = "Velha Cidade",
                Estado = "XX",
                Capacidade = 10
            };

            _patioRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(patio);
            _patioRepositoryMock.Setup(r => r.UpdateAsync(patio)).Returns(Task.CompletedTask);

            var updateDto = new UpdatePatioDto
            {
                Cep = "99999-999",
                Numero = "50",
                Cidade = "Nova Cidade",
                Estado = "YY",
                Capacidade = 99
            };

            _patioRepositoryMock.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(() => patio);

            var result = await _service.UpdateAsync(id, updateDto);

            result.Should().NotBeNull();
            result!.Cidade.Should().Be("Nova Cidade");
            _patioRepositoryMock.Verify(r => r.UpdateAsync(patio), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenPatioNotFound()
        {
            var id = Guid.NewGuid();
            _patioRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Patio?)null);

            var result = await _service.UpdateAsync(id, new UpdatePatioDto());

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAndReturnTrue_WhenPatioExists()
        {
            var id = Guid.NewGuid();
            var patio = new Patio { Id = id };

            _patioRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(patio);
            _patioRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(id);

            result.Should().BeTrue();
            _patioRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenPatioNotFound()
        {
            var id = Guid.NewGuid();
            _patioRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Patio?)null);

            var result = await _service.DeleteAsync(id);

            result.Should().BeFalse();
            _patioRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
