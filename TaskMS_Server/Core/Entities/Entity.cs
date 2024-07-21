namespace Core.Entities;

//abstract: tamamen kalıtım amaçlı kullanılan temel sınıf

public abstract class Entity<TId> : IEntity<TId> 
{
    public TId Id { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}