namespace Sample.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly IRepository<Todo> _todoRepo;
        private readonly IEmailService _emailService;
        private readonly ILogger<TodoController> _logger;
        private readonly IMapper _mapper;

        public TodoController(IRepository<Todo> todoRepo, IEmailService emailService, ILogger<TodoController> logger, IMapper mapper)
        {
            _todoRepo = todoRepo;
            _emailService = emailService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<TodoDTO>>(await _todoRepo.GetAllAsync()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(_mapper.Map<TodoDTO>(await _todoRepo.GetByIdAsync(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] TodoDTO todo)
        {
            if (todo == null) return BadRequest("No ToDo items.");

            bool result = await _todoRepo.AddAsync(_mapper.Map<Todo>(todo));

            if (result) return Ok("Add successful");

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TodoDTO newTodo)
        {
            if (id < 1 || newTodo == null) return BadRequest("Unknown item, or update incorrect");

            Todo existingTodo = await _todoRepo.GetByIdAsync(id);

            _mapper.Map(newTodo, existingTodo);

            bool result = await _todoRepo.UpdateAsync(existingTodo);

            if (result) return Ok("Update successful");

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1) return BadRequest("Unknown item");

            Todo todo = await _todoRepo.GetByIdAsync(id);

            if (todo == null) return BadRequest("Item not found");

            bool result = await _todoRepo.DeleteAsync(todo);

            if (result) return Ok("Delete successful");

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
