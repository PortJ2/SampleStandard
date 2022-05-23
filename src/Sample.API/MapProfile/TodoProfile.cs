namespace Sample.API.MapProfile
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<Todo, TodoDTO>().ReverseMap();
        }
    }
}
