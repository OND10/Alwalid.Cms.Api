using AutoMapper;

namespace ChatApp.Application.Common.Mappings;

/// <summary>
/// Interface for types that can be mapped from another type
/// </summary>
/// <typeparam name="T">The source type to map from</typeparam>
public interface IMapFrom<T>
{
    /// <summary>
    /// Creates the mapping configuration
    /// </summary>
    /// <param name="profile">The AutoMapper profile</param>
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
}