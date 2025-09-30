using Core.Iottu.Domain.Entities;
using Core.Iottu.Domain.Interfaces;
using Shared.Iottu.Contracts.DTOs;

namespace Core.Iottu.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<TagDto>> GetAllAsync(int page, int pageSize)
        {
            var tags = await _tagRepository.GetAllAsync(page, pageSize);
            return tags.Select(t => new TagDto
            {
                Id = t.Id,
                CodigoRFID = t.CodigoRFID,
                SSIDWifi = t.SSIDWifi,
                Latitude = t.Latitude,
                Longitude = t.Longitude,
                DataHora = t.DataHora,
                AntenaId = t.AntenaId,
            });
        }

        public async Task<TagDto?> GetByIdAsync(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return null;

            return new TagDto
            {
                Id = tag.Id,
                CodigoRFID = tag.CodigoRFID,
                SSIDWifi = tag.SSIDWifi,
                Latitude = tag.Latitude,
                Longitude = tag.Longitude,
                DataHora = tag.DataHora,
                AntenaId = tag.AntenaId,
            };
        }

        public async Task<TagDto> CreateAsync(CreateTagDto dto)
        {
            var tag = new Tag(dto.CodigoRFID, dto.SSIDWifi, dto.Latitude, dto.Longitude, dto.DataHora, dto.AntenaId);
            await _tagRepository.AddAsync(tag);

            return new TagDto
            {
                Id = tag.Id,
                CodigoRFID = tag.CodigoRFID,
                SSIDWifi = tag.SSIDWifi,
                Latitude = tag.Latitude,
                Longitude = tag.Longitude,
                DataHora = tag.DataHora,
                AntenaId = tag.AntenaId,
            };
        }

        public async Task<TagDto?> UpdateAsync(Guid id, UpdateTagDto dto)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return null;

            typeof(Tag).GetProperty("CodigoRFID")!.SetValue(tag, dto.CodigoRFID);
            typeof(Tag).GetProperty("SSIDWifi")!.SetValue(tag, dto.SSIDWifi);
            typeof(Tag).GetProperty("Latitude")!.SetValue(tag, dto.Latitude);
            typeof(Tag).GetProperty("Longitude")!.SetValue(tag, dto.Longitude);
            typeof(Tag).GetProperty("DataHora")!.SetValue(tag, dto.DataHora);
            typeof(Tag).GetProperty("AntenaId")!.SetValue(tag, dto.AntenaId);

            await _tagRepository.UpdateAsync(tag);

            return new TagDto
            {
                Id = tag.Id,
                CodigoRFID = tag.CodigoRFID,
                SSIDWifi = tag.SSIDWifi,
                Latitude = tag.Latitude,
                Longitude = tag.Longitude,
                DataHora = tag.DataHora,
                AntenaId = tag.AntenaId,
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return false;

            await _tagRepository.DeleteAsync(id);
            return true;
        }
    }
}
