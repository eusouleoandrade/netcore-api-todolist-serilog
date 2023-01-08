using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Common
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseEntity<TId>
        where TId : struct
    {
        [Key]
        public TId Id { get; protected set; }
    }
}