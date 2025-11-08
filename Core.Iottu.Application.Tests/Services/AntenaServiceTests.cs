using Core.Iottu.Application.Services;
using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared.Iottu.Contracts.DTOs;
using Xunit;

namespace Core.Iottu.Application.Tests.Services
{
    public class AntenaServiceTests
    {
        private readonly Mock<IAntenaRepository> _antenaRepositoryMock;
        private readonly AntenaService _service;

        public AntenaServiceTests()
        {
            _antenaRepositoryMock = new Mock<IAntenaRepository>();
            _service = new AntenaService(_antenaRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            var antenas = new List<Antena>
            {
                new Antena("Pátio A", "ANT-001"),
                new Antena("Pátio B", "ANT-002")
            };

            _antenaRepositoryMock.Setup(r => r.GetAllAsync(1, 10)).ReturnsAsync(antenas);

            var result = (await _service.GetAllAsync(1, 10)).ToList();

            result.Should().HaveCount(2);
            result[0].Localizacao.Should().Be("Pátio A");
            result[1].Identificador.Should().Be("ANT-002");
        }

        [Fact]
        public async Task CountAsync_ShouldReturnRepositoryCount()
        {
            _antenaRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(7);

            var result = await _service.CountAsync();

            result.Should().Be(7);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenFound()
        {
            var id = Guid.NewGuid();
            var antena = new Antena("Depósito Central", "ANT-777");
            typeof(Antena).GetProperty("Id")!.SetValue(antena, id);
            _antenaRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(antena);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result!.Localizacao.Should().Be("Depósito Central");
            result.Identificador.Should().Be("ANT-777");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _antenaRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Antena?)null);

            var result = await _service.GetByIdAsync(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAndReturnNewAntena()
        {
            var dto = new CreateAntenaDto
            {
                Localizacao = "Galpão 3",
                Identificador = "ANT-003"
            };

            Antena? stored = null;

            _antenaRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Antena>()))
                .Callback<Antena>(a => stored = a)
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.Localizacao.Should().Be("Galpão 3");
            stored.Should().NotBeNull();
            stored!.Identificador.Should().Be("ANT-003");
            _antenaRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Antena>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExistingAntena()
        {
            var id = Guid.NewGuid();
            var existing = new Antena("Local Antigo", "ANT-OLD");
            typeof(Antena).GetProperty("Id")!.SetValue(existing, id);
            _antenaRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            var dto = new UpdateAntenaDto
            {
                Localizacao = "Local Novo",
                Identificador = "ANT-NEW"
            };

            _antenaRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Antena>()))
                .Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(id, dto);

            result.Should().NotBeNull();
            result!.Localizacao.Should().Be("Local Novo");
            result.Identificador.Should().Be("ANT-NEW");
            _antenaRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Antena>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenAntenaNotFound()
        {
            var id = Guid.NewGuid();
            _antenaRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Antena?)null);

            var result = await _service.UpdateAsync(id, new UpdateAntenaDto());

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAndReturnTrue_WhenAntenaExists()
        {
            var id = Guid.NewGuid();
            var antena = new Antena("Estoque", "ANT-DEL");
            typeof(Antena).GetProperty("Id")!.SetValue(antena, id);
            _antenaRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(antena);
            _antenaRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(id);

            result.Should().BeTrue();
            _antenaRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenAntenaNotFound()
        {
            var id = Guid.NewGuid();
            _antenaRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Antena?)null);

            var result = await _service.DeleteAsync(id);

            result.Should().BeFalse();
            _antenaRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
