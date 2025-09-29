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
                Codigo = t.Codigo,
                Ativa = t.Ativa
            });
        }

        public async Task<TagDto?> GetByIdAsync(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return null;

            return new TagDto
            {
                Id = tag.Id,
                Codigo = tag.Codigo,
                Ativa = tag.Ativa
            };
        }

        public async Task<TagDto> CreateAsync(CreateTagDto dto)
        {
            var tag = new Tag(dto.Codigo);
            await _tagRepository.AddAsync(tag);

            return new TagDto
            {
                Id = tag.Id,
                Codigo = tag.Codigo,
                Ativa = tag.Ativa
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return false;

            await _tagRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> AtivarAsync(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return false;

            tag.Ativar();
            await _tagRepository.UpdateAsync(tag);
            return true;
        }

        public async Task<bool> DesativarAsync(Guid id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag == null) return false;

            tag.Desativar();
            await _tagRepository.UpdateAsync(tag);
            return true;
        }
    }
}
