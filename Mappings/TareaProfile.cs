using AutoMapper;
using TareasApi.Dtos;
using TareasApi.Models;

namespace TareasApi.Mappings
{
    public class TareaProfile : Profile
    {
        public TareaProfile()
        {
            // Model → DTO
            CreateMap<Tarea, TareaDto>();

            // DTO → Model (al crear una nueva tarea)
            CreateMap<TareaCreateDto, Tarea>();

            // DTO → Model (al editar una tarea)
            CreateMap<TareaUpdateDto, Tarea>()
                .ForMember(dest => dest.UsuarioId, opt => opt.Ignore()); // el usuario lo controla el backend
        }
    }
}
