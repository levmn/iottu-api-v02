namespace Shared.Iottu.Contracts.DTOs
{
    /// <summary>
    /// Envelope de resposta paginada, com metadados e links HATEOAS para navegação.
    /// </summary>
    /// <typeparam name="T">Tipo dos itens retornados.</typeparam>
    public class PagedResponse<T>
    {
        /// <summary>
        /// Número da página atual (>= 1).
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Tamanho da página (itens por página, >= 1).
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Quantidade total de itens disponíveis no recurso.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Quantidade total de páginas baseadas no <see cref="TotalItems"/> e <see cref="PageSize"/>.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Coleção de itens da página atual.
        /// </summary>
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();

        /// <summary>
        /// Links HATEOAS no nível da coleção (ex.: self, next, prev).
        /// </summary>
        public IEnumerable<object>? Links { get; set; }
    }
}


