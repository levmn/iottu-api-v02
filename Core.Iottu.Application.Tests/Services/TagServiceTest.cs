using Core.Iottu.Application.Services;
using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Shared.Iottu.Contracts.DTOs;
using Xunit;

namespace Core.Iottu.Application.Tests.Services
{
    public class TagServiceTests
    {
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly TagService _service;

        public TagServiceTests()
        {
            _tagRepositoryMock = new Mock<ITagRepository>();
            _service = new TagService(_tagRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedDtos()
        {
            var tags = new List<Tag>
            {
                new Tag("RFID001", "SSID-A", -27.59, -48.55, DateTime.UtcNow, Guid.NewGuid()),
                new Tag("RFID002", "SSID-B", -22.90, -43.17, DateTime.UtcNow.AddMinutes(-5), Guid.NewGuid())
            };

            _tagRepositoryMock.Setup(r => r.GetAllAsync(1, 10)).ReturnsAsync(tags);

            var result = (await _service.GetAllAsync(1, 10)).ToList();

            result.Should().HaveCount(2);
            result[0].CodigoRFID.Should().Be("RFID001");
            result[1].SSIDWifi.Should().Be("SSID-B");
        }

        [Fact]
        public async Task CountAsync_ShouldReturnRepositoryCount()
        {
            _tagRepositoryMock.Setup(r => r.CountAsync()).ReturnsAsync(12);

            var result = await _service.CountAsync();

            result.Should().Be(12);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenFound()
        {
            var id = Guid.NewGuid();
            var tag = new Tag("RFID123", "SSID-1", -23.5, -46.6, DateTime.UtcNow, Guid.NewGuid());
            typeof(Tag).GetProperty("Id")!.SetValue(tag, id);

            _tagRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tag);

            var result = await _service.GetByIdAsync(id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(id);
            result.CodigoRFID.Should().Be("RFID123");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotFound()
        {
            var id = Guid.NewGuid();
            _tagRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Tag?)null);

            var result = await _service.GetByIdAsync(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldAddAndReturnNewTag()
        {
            var dto = new CreateTagDto
            {
                CodigoRFID = "RFID999",
                SSIDWifi = "SSID-Z",
                Latitude = -25.43,
                Longitude = -49.27,
                DataHora = DateTime.UtcNow,
                AntenaId = Guid.NewGuid()
            };

            Tag? stored = null;
            _tagRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Tag>()))
                .Callback<Tag>(t => stored = t)
                .Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.CodigoRFID.Should().Be("RFID999");
            stored.Should().NotBeNull();
            _tagRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Tag>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExistingTag()
        {
            // Arrange
            var id = Guid.NewGuid();
            var tag = new Tag("RFID111", "SSID-OLD", -22.0, -43.0, DateTime.UtcNow, Guid.NewGuid());
            typeof(Tag).GetProperty("Id")!.SetValue(tag, id);
            _tagRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tag);

            var dto = new UpdateTagDto
            {
                CodigoRFID = "RFID222",
                SSIDWifi = "SSID-NEW",
                Latitude = -20.1,
                Longitude = -40.5,
                DataHora = DateTime.UtcNow.AddHours(1),
                AntenaId = Guid.NewGuid()
            };

            _tagRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Tag>())).Returns(Task.CompletedTask);

            var result = await _service.UpdateAsync(id, dto);

            result.Should().NotBeNull();
            result!.CodigoRFID.Should().Be("RFID222");
            result.SSIDWifi.Should().Be("SSID-NEW");
            _tagRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Tag>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnNull_WhenTagNotFound()
        {
            var id = Guid.NewGuid();
            _tagRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Tag?)null);

            var result = await _service.UpdateAsync(id, new UpdateTagDto());

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAndReturnTrue_WhenTagExists()
        {
            var id = Guid.NewGuid();
            var tag = new Tag("RFIDDEL", "SSID-DEL", 0, 0, DateTime.UtcNow, Guid.NewGuid());
            typeof(Tag).GetProperty("Id")!.SetValue(tag, id);

            _tagRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(tag);
            _tagRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

            var result = await _service.DeleteAsync(id);

            result.Should().BeTrue();
            _tagRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenTagNotFound()
        {
            var id = Guid.NewGuid();
            _tagRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Tag?)null);

            var result = await _service.DeleteAsync(id);

            result.Should().BeFalse();
            _tagRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
